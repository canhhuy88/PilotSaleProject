using BizI.Application.Common;
using BizI.Application.Interfaces;

namespace BizI.Application.Features.Inventory.ReturnStock;

public class ReturnStockHandler : IRequestHandler<ReturnStockCommand, CommandResult>
{
    private readonly IInventoryService _inventoryService;

    public ReturnStockHandler(IInventoryService inventoryService)
    {
        _inventoryService = inventoryService;
    }

    public async Task<CommandResult> Handle(ReturnStockCommand request, CancellationToken cancellationToken)
    {
        await _inventoryService.ReturnStockAsync(request.ProductId, request.WarehouseId, request.Quantity, request.ReferenceId);
        return CommandResult.SuccessResult();
    }
}
