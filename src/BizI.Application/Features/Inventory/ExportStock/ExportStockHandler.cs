using BizI.Application.Common;
using BizI.Application.Interfaces;

namespace BizI.Application.Features.Inventory.ExportStock;

public class ExportStockHandler : IRequestHandler<ExportStockCommand, CommandResult>
{
    private readonly IInventoryService _inventoryService;

    public ExportStockHandler(IInventoryService inventoryService)
    {
        _inventoryService = inventoryService;
    }

    public async Task<CommandResult> Handle(ExportStockCommand request, CancellationToken cancellationToken)
    {
        await _inventoryService.ExportStockAsync(request.ProductId, request.WarehouseId, request.Quantity, request.ReferenceId);
        return CommandResult.SuccessResult();
    }
}
