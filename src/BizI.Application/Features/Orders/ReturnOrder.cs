namespace BizI.Application.Features.Orders;

public record ReturnOrderCommand(Guid OrderId, List<OrderItemDto> ItemsToReturn, Guid WarehouseId) : IRequest<CommandResult>;

public class ReturnOrderCommandValidator : AbstractValidator<ReturnOrderCommand>
{
    public ReturnOrderCommandValidator()
    {
        RuleFor(x => x.OrderId).NotEmpty();
        RuleFor(x => x.WarehouseId).NotEmpty();
        RuleFor(x => x.ItemsToReturn).NotEmpty();
        RuleForEach(x => x.ItemsToReturn).ChildRules(item =>
        {
            item.RuleFor(x => x.ProductId).NotEmpty();
            item.RuleFor(x => x.Quantity).GreaterThan(0);
        });
    }
}

public class ReturnOrderHandler : IRequestHandler<ReturnOrderCommand, CommandResult>
{
    private readonly IRepository<Order> _orderRepo;
    private readonly IInventoryService _inventoryService;

    public ReturnOrderHandler(IRepository<Order> orderRepo, IInventoryService inventoryService)
    {
        _orderRepo = orderRepo;
        _inventoryService = inventoryService;
    }

    public async Task<CommandResult> Handle(ReturnOrderCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var order = await _orderRepo.GetByIdAsync(request.OrderId.ToString());
            if (order == null) return CommandResult.FailureResult("Order not found");

            if (order.Status == OrderStatus.Returned.ToString())
                return CommandResult.FailureResult("Order is already fully returned");

            foreach (var returnItem in request.ItemsToReturn)
            {
                var originalItem = order.Items.FirstOrDefault(x => x.ProductId == returnItem.ProductId);
                if (originalItem == null)
                    return CommandResult.FailureResult($"Product {returnItem.ProductId} was not in original order");

                int remainingToReturn = originalItem.Quantity - originalItem.ReturnedQuantity;
                if (returnItem.Quantity > remainingToReturn)
                {
                    throw new OverReturnException(returnItem.ProductId, returnItem.Quantity, remainingToReturn);
                }

                await _inventoryService.ReturnStockAsync(returnItem.ProductId, request.WarehouseId, returnItem.Quantity, Guid.Parse(order.Id));

                originalItem.ReturnedQuantity += returnItem.Quantity;
            }

            bool allReturned = order.Items.All(x => x.Quantity == x.ReturnedQuantity);
            order.Status = allReturned ? OrderStatus.Returned.ToString() : order.Status;

            await _orderRepo.UpdateAsync(order);

            return CommandResult.SuccessResult(new
            {
                OrderId = order.Id,
                Status = order.Status.ToString(),
                RemainingItems = order.Items.Select(x => new { x.ProductId, Remaining = x.Quantity - x.ReturnedQuantity })
            });
        }
        catch (DomainException ex)
        {
            return CommandResult.FailureResult(ex.Message);
        }
        catch (Exception ex)
        {
            return CommandResult.FailureResult("An error occurred while processing the return: " + ex.Message);
        }
    }
}
