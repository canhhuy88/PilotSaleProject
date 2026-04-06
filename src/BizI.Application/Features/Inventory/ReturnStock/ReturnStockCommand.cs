using BizI.Application.Common;

namespace BizI.Application.Features.Inventory.ReturnStock;

public record ReturnStockCommand(
    Guid ProductId,
    Guid WarehouseId,
    int Quantity,
    Guid? ReferenceId = null) : IRequest<CommandResult>;
