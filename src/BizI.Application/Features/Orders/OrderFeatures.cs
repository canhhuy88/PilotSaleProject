using BizI.Application.DTOs.Order;
using BizI.Domain.Exceptions;
using Microsoft.Extensions.Logging;

namespace BizI.Application.Features.Orders;

// ── Commands/Queries ─────────────────────────────────────────────────────────

public record CreateOrderCommand(
    string CustomerId,
    List<CreateOrderItemDto> Items,
    string WarehouseId,
    decimal Discount = 0m,
    string Currency = "VND") : IRequest<CommandResult>;

public record DeleteOrderCommand(string Id) : IRequest<CommandResult>;

public record CompleteOrderCommand(string OrderId) : IRequest<CommandResult>;

public record CancelOrderCommand(string OrderId) : IRequest<CommandResult>;

public record ReturnOrderCommand(
    string OrderId,
    string WarehouseId,
    List<ReturnItemDto> Items) : IRequest<CommandResult>;

public record GetAllOrdersQuery : IRequest<IEnumerable<OrderDto>>;

public record GetOrderQuery(string OrderId) : IRequest<OrderDto?>;

// ── Validators ───────────────────────────────────────────────────────────────

public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(x => x.CustomerId).NotEmpty();
        RuleFor(x => x.WarehouseId).NotEmpty();
        RuleFor(x => x.Items).NotEmpty().WithMessage("Order must have at least one item.");
        RuleFor(x => x.Discount).GreaterThanOrEqualTo(0);

        RuleForEach(x => x.Items).ChildRules(item =>
        {
            item.RuleFor(x => x.ProductId).NotEmpty();
            item.RuleFor(x => x.Quantity).GreaterThan(0);
            item.RuleFor(x => x.Price).GreaterThanOrEqualTo(0);
        });
    }
}

public class ReturnOrderCommandValidator : AbstractValidator<ReturnOrderCommand>
{
    public ReturnOrderCommandValidator()
    {
        RuleFor(x => x.OrderId).NotEmpty();
        RuleFor(x => x.WarehouseId).NotEmpty();
        RuleFor(x => x.Items).NotEmpty();

        RuleForEach(x => x.Items).ChildRules(item =>
        {
            item.RuleFor(x => x.ProductId).NotEmpty();
            item.RuleFor(x => x.Quantity).GreaterThan(0);
        });
    }
}

// ── Handlers ─────────────────────────────────────────────────────────────────

public class CreateOrderHandler : IRequestHandler<CreateOrderCommand, CommandResult>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IInventoryService _inventoryService;
    private readonly ILogger<CreateOrderHandler> _logger;

    public CreateOrderHandler(
        IOrderRepository orderRepository,
        IInventoryService inventoryService,
        ILogger<CreateOrderHandler> logger)
    {
        _orderRepository = orderRepository;
        _inventoryService = inventoryService;
        _logger = logger;
    }

    public async Task<CommandResult> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // ✅ Build domain OrderItem child entities via Domain factory
            var lineItems = request.Items
                .Select(i => OrderItem.Create(i.ProductId, i.Quantity, i.Price, request.Currency))
                .ToList();

            // Generate a unique order code
            var code = $"ORD-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString("N")[..6].ToUpper()}";

            // ✅ Create the Order aggregate — enforces business rules internally
            var order = Order.Create(
                code,
                request.CustomerId,
                createdBy: string.Empty,
                lineItems,
                request.Discount,
                request.Currency);

            // ✅ Export stock for each line item using interface (no direct DB)
            foreach (var item in request.Items)
            {
                await _inventoryService.ExportStockAsync(
                    item.ProductId, request.WarehouseId, item.Quantity, order.Id);
            }

            // ✅ Mark order as completed after successful stock export using domain method
            order.Complete();

            await _orderRepository.AddAsync(order);

            _logger.LogInformation("Order created. Id: {OrderId}, Code: {Code}", order.Id, order.Code);
            return CommandResult.SuccessResult(order.Id);
        }
        catch (DomainException ex)
        {
            _logger.LogWarning("Domain rule violated creating order: {Message}", ex.Message);
            return CommandResult.FailureResult(ex.Message);
        }
    }
}

public class DeleteOrderHandler : IRequestHandler<DeleteOrderCommand, CommandResult>
{
    private readonly IOrderRepository _orderRepository;
    private readonly ILogger<DeleteOrderHandler> _logger;

    public DeleteOrderHandler(IOrderRepository orderRepository, ILogger<DeleteOrderHandler> logger)
    {
        _orderRepository = orderRepository;
        _logger = logger;
    }

    public async Task<CommandResult> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(request.Id);
        if (order is null)
            return CommandResult.FailureResult($"Order '{request.Id}' not found.");

        // ✅ Use Domain method to enforce cancellation rules
        try { order.Cancel(); }
        catch (DomainException ex) { return CommandResult.FailureResult(ex.Message); }

        await _orderRepository.UpdateAsync(order);
        _logger.LogInformation("Order cancelled/deleted. Id: {Id}", request.Id);
        return CommandResult.SuccessResult(request.Id);
    }
}

public class ReturnOrderHandler : IRequestHandler<ReturnOrderCommand, CommandResult>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IInventoryService _inventoryService;
    private readonly ILogger<ReturnOrderHandler> _logger;

    public ReturnOrderHandler(
        IOrderRepository orderRepository,
        IInventoryService inventoryService,
        ILogger<ReturnOrderHandler> logger)
    {
        _orderRepository = orderRepository;
        _inventoryService = inventoryService;
        _logger = logger;
    }

    public async Task<CommandResult> Handle(ReturnOrderCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var order = await _orderRepository.GetByIdAsync(request.OrderId);
            if (order is null)
                return CommandResult.FailureResult("Order not found.");

            foreach (var returnItem in request.Items)
            {
                var originalItem = order.Items
                    .FirstOrDefault(x => x.ProductId == returnItem.ProductId);

                if (originalItem is null)
                    return CommandResult.FailureResult(
                        $"Product {returnItem.ProductId} was not in the original order.");

                // ✅ Call domain method — throws DomainException if over-returning
                originalItem.Return(returnItem.Quantity);

                // ✅ Return stock via application service (no direct DB access)
                await _inventoryService.ReturnStockAsync(
                    returnItem.ProductId, request.WarehouseId,
                    returnItem.Quantity, order.Id);
            }

            // ✅ Check if all items fully returned → mark via Domain method
            bool allReturned = order.Items.All(x => x.RemainingQuantity == 0);
            if (allReturned)
                order.MarkAsReturned();

            await _orderRepository.UpdateAsync(order);

            _logger.LogInformation("Return processed for order {OrderId}", request.OrderId);

            return CommandResult.SuccessResult(new
            {
                OrderId = order.Id,
                Status = order.Status.ToString(),
                RemainingItems = order.Items
                    .Select(x => new { x.ProductId, Remaining = x.RemainingQuantity })
            });
        }
        catch (DomainException ex)
        {
            _logger.LogWarning("Domain exception during return: {Message}", ex.Message);
            return CommandResult.FailureResult(ex.Message);
        }
    }
}

public class GetAllOrdersHandler : IRequestHandler<GetAllOrdersQuery, IEnumerable<OrderDto>>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IMapper _mapper;

    public GetAllOrdersHandler(IOrderRepository orderRepository, IMapper mapper)
    {
        _orderRepository = orderRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<OrderDto>> Handle(GetAllOrdersQuery request, CancellationToken cancellationToken)
    {
        var orders = await _orderRepository.GetAllAsync();

        // ✅ Return DTOs — never raw entities
        return _mapper.Map<IEnumerable<OrderDto>>(orders);
    }
}

public class GetOrderHandler : IRequestHandler<GetOrderQuery, OrderDto?>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IMapper _mapper;

    public GetOrderHandler(IOrderRepository orderRepository, IMapper mapper)
    {
        _orderRepository = orderRepository;
        _mapper = mapper;
    }

    public async Task<OrderDto?> Handle(GetOrderQuery request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(request.OrderId);
        return order is null ? null : _mapper.Map<OrderDto>(order);
    }
}
