namespace BizI.Application.Features.Inventory;

public record AdjustStockCommand(Guid ProductId, Guid WarehouseId, int Quantity) : IRequest<CommandResult>;

public class AdjustStockHandler : IRequestHandler<AdjustStockCommand, CommandResult>
{
    private readonly IInventoryService _inventoryService;

    public AdjustStockHandler(IInventoryService inventoryService)
    {
        _inventoryService = inventoryService;
    }

    public async Task<CommandResult> Handle(AdjustStockCommand request, CancellationToken cancellationToken)
    {
        await _inventoryService.AdjustStockAsync(request.ProductId, request.WarehouseId, request.Quantity);
        return CommandResult.SuccessResult();
    }
}
