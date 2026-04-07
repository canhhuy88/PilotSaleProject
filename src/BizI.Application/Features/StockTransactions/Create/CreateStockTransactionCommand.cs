using BizI.Domain.Entities;

namespace BizI.Application.Features.StockTransactions.Create;

public record CreateStockTransactionCommand(
    StockTransactionType Type,
    Guid ProductId,
    Guid WarehouseId,
    int Quantity,
    int BeforeQty,
    int AfterQty,
    Guid RefId,
    string CreatedBy
) : IRequest<CommandResult>;
