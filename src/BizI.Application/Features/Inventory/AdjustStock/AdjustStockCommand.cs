using BizI.Application.Common;

namespace BizI.Application.Features.Inventory.AdjustStock;

public record AdjustStockCommand(
    Guid ProductId,
    Guid WarehouseId,
    int NewQuantity) : IRequest<CommandResult>;
