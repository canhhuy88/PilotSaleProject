using BizI.Application.Common;

namespace BizI.Application.Features.Inventory.ExportStock;

public record ExportStockCommand(
    Guid ProductId,
    Guid WarehouseId,
    int Quantity,
    Guid? ReferenceId = null) : IRequest<CommandResult>;
