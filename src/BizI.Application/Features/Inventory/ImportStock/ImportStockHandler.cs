using BizI.Application.Common;
using BizI.Application.Interfaces;

namespace BizI.Application.Features.Inventory.ImportStock;

public class ImportStockHandler : IRequestHandler<ImportStockCommand, CommandResult>
{
    private readonly IInventoryService _inventoryService;

    public ImportStockHandler(IInventoryService inventoryService)
    {
        _inventoryService = inventoryService;
    }

    public async Task<CommandResult> Handle(ImportStockCommand request, CancellationToken cancellationToken)
    {
        await _inventoryService.ImportStockAsync(request.ProductId, request.WarehouseId, request.Quantity, request.ReferenceId);
        return CommandResult.SuccessResult();
    }
}
