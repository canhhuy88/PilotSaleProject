using BizI.Domain.Enums;
using BizI.Domain.Exceptions;

namespace BizI.Domain.Entities;

/// <summary>
/// Child entity of the Inventory aggregate.
/// Provides an immutable audit trail of every stock movement.
/// </summary>
public class InventoryTransaction : BaseEntity
{
    public string ProductId { get; private set; } = string.Empty;
    public string WarehouseId { get; private set; } = string.Empty;
    public InventoryTransactionType Type { get; private set; }
    public int Quantity { get; private set; }

    /// <summary>Optional reference to the originating Order or ImportOrder.</summary>
    public string? ReferenceId { get; private set; }

    private InventoryTransaction() { } // ORM / serialization

    // ──────────────────────────────────────────────
    //  Factory
    // ──────────────────────────────────────────────

    /// <summary>Records a single stock movement event.</summary>
    public static InventoryTransaction Create(
        string productId,
        string warehouseId,
        InventoryTransactionType type,
        int quantity,
        string? referenceId = null)
    {
        if (string.IsNullOrWhiteSpace(productId))
            throw new DomainException("ProductId cannot be empty.");

        if (string.IsNullOrWhiteSpace(warehouseId))
            throw new DomainException("WarehouseId cannot be empty.");

        if (quantity == 0)
            throw new DomainException("Transaction quantity cannot be zero.");

        return new InventoryTransaction
        {
            ProductId = productId.Trim(),
            WarehouseId = warehouseId.Trim(),
            Type = type,
            Quantity = quantity,
            ReferenceId = referenceId?.Trim()
        };
    }
}
