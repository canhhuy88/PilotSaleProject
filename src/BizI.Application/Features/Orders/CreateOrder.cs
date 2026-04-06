namespace BizI.Application.Features.Orders;

public record OrderItemDto(Guid ProductId, int Quantity, decimal Price);

public record CreateOrderCommand(List<OrderItemDto> Items, Guid WarehouseId) : IRequest<CommandResult>;

public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(x => x.WarehouseId).NotEmpty();
        RuleFor(x => x.Items).NotEmpty().WithMessage("Order must have at least one item");
        RuleForEach(x => x.Items).ChildRules(item =>
        {
            item.RuleFor(x => x.ProductId).NotEmpty();
            item.RuleFor(x => x.Quantity).GreaterThan(0);
            item.RuleFor(x => x.Price).GreaterThanOrEqualTo(0);
        });
    }
}

public class CreateOrderHandler : IRequestHandler<CreateOrderCommand, CommandResult>
{
    private readonly IRepository<Order> _orderRepo;
    private readonly IInventoryService _inventoryService;

    public CreateOrderHandler(IRepository<Order> orderRepo, IInventoryService inventoryService)
    {
        _orderRepo = orderRepo;
        _inventoryService = inventoryService;
    }

    public async Task<CommandResult> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var orderId = Guid.NewGuid();

            foreach (var item in request.Items)
            {
                await _inventoryService.ExportStockAsync(item.ProductId, request.WarehouseId, item.Quantity, orderId);
            }

            var order = new Order
            {
                Id = orderId.ToString(),
                TotalAmount = request.Items.Sum(x => x.Price * x.Quantity),
                Status = OrderStatus.Completed.ToString(),
                Items = request.Items.Select(x => new OrderItem
                {
                    ProductId = x.ProductId,
                    Quantity = x.Quantity,
                    Price = x.Price
                }).ToList()
            };

            await _orderRepo.AddAsync(order);

            return CommandResult.SuccessResult(order.Id);
        }
        catch (DomainException ex)
        {
            return CommandResult.FailureResult(ex.Message);
        }
        catch (Exception ex)
        {
            return CommandResult.FailureResult("An error occurred while creating the order: " + ex.Message);
        }
    }
}
