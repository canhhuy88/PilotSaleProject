using BizI.Domain.Enums;

namespace BizI.Application.Features.InventoryTransactions.Dtos;

/// <summary>
/// Read DTO for an inventory transaction (stock movement audit trail).
/// </summary>
public record InventoryTransactionDto(
    Guid Id,
    Guid ProductId,
    Guid WarehouseId,
    InventoryTransactionType Type,
    int Quantity,
    Guid? ReferenceId,
    DateTime CreatedAt);
