using Microsoft.Extensions.Logging;

namespace BizI.Application.Features.Orders.Create;

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
