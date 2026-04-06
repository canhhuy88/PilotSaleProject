using BizI.Application.Features.PaymentAndReturns.Dtos;

namespace BizI.Application.Features.PaymentAndReturns.CreateReturnOrder;

public class CreateReturnOrderHandler : IRequestHandler<CreateReturnOrderCommand, CommandResult>
{
    private readonly IRepository<ReturnOrder> _repo;
    private readonly IOrderRepository _orderRepo;
    private readonly IInventoryService _inventoryService;

    public CreateReturnOrderHandler(
        IRepository<ReturnOrder> repo,
        IOrderRepository orderRepo,
        IInventoryService inventoryService)
    {
        _repo = repo;
        _orderRepo = orderRepo;
        _inventoryService = inventoryService;
    }

    public async Task<CommandResult> Handle(CreateReturnOrderCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var order = await _orderRepo.GetByIdAsync(request.OrderId);
            if (order is null) return CommandResult.FailureResult("Order not found.");

            // ✅ Build domain ReturnItem child entities
            var returnItems = request.Items
                .Select(i => ReturnItem.Create(i.ProductId, i.Quantity, i.RefundPrice, request.Currency))
                .ToList();

            // ✅ Domain factory validates the return order
            var returnOrder = ReturnOrder.Create(request.OrderId, returnItems, request.Currency);

            // Return stock for each item
            foreach (var item in request.Items)
            {
                await _inventoryService.ReturnStockAsync(
                    item.ProductId, request.WarehouseId,
                    item.Quantity, request.OrderId);
            }

            // ✅ Mark original order as returned via domain method
            order.MarkAsReturned();
            await _orderRepo.UpdateAsync(order);

            await _repo.AddAsync(returnOrder);
            return CommandResult.SuccessResult(returnOrder.Id);
        }
        catch (DomainException ex) { return CommandResult.FailureResult(ex.Message); }
    }
}
