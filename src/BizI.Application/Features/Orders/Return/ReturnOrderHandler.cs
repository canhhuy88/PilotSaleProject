using Microsoft.Extensions.Logging;

namespace BizI.Application.Features.Orders.Return;

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
