namespace BizI.Application.Features.Inventory;

public record ImportStockCommand(Guid ProductId, Guid WarehouseId, int Quantity) : IRequest<CommandResult>;

public class ImportStockHandler : IRequestHandler<ImportStockCommand, CommandResult>
{
    private readonly IInventoryService _inventoryService;

    public ImportStockHandler(IInventoryService inventoryService)
    {
        _inventoryService = inventoryService;
    }

    public async Task<CommandResult> Handle(ImportStockCommand request, CancellationToken cancellationToken)
    {
        await _inventoryService.ImportStockAsync(request.ProductId, request.WarehouseId, request.Quantity);
        return CommandResult.SuccessResult();
    }
}
