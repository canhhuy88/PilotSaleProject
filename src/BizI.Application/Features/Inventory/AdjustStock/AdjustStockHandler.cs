using BizI.Application.Common;
using BizI.Application.Interfaces;

namespace BizI.Application.Features.Inventory.AdjustStock;

public class AdjustStockHandler : IRequestHandler<AdjustStockCommand, CommandResult>
{
    private readonly IInventoryService _inventoryService;

    public AdjustStockHandler(IInventoryService inventoryService)
    {
        _inventoryService = inventoryService;
    }

    public async Task<CommandResult> Handle(AdjustStockCommand request, CancellationToken cancellationToken)
    {
        await _inventoryService.AdjustStockAsync(request.ProductId, request.WarehouseId, request.NewQuantity);
        return CommandResult.SuccessResult();
    }
}
