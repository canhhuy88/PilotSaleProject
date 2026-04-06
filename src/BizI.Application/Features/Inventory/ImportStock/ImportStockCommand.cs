using BizI.Application.Common;

namespace BizI.Application.Features.Inventory.ImportStock;

public record ImportStockCommand(
    Guid ProductId,
    Guid WarehouseId,
    int Quantity,
    Guid? ReferenceId = null) : IRequest<CommandResult>;
