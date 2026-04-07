using BizI.Domain.Enums;

namespace BizI.Application.Features.StockTransactions.Dtos;

public record StockTransactionDto(
    Guid Id,
    Guid ProductId,
    Guid WarehouseId,
    string Type,
    int Quantity,
    int BeforeQty,
    int AfterQty,
    Guid RefId,
    string CreatedBy,
    DateTime CreatedAt
);
