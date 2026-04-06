using BizI.Domain.Enums;
using BizI.Domain.Exceptions;

namespace BizI.Domain.Entities;

/// <summary>
/// Child entity of the Inventory aggregate.
/// Provides an immutable audit trail of every stock movement.
/// </summary>
public class InventoryTransaction : BaseEntity
{
    public Guid ProductId { get; private set; }
    public Guid WarehouseId { get; private set; }
    public InventoryTransactionType Type { get; private set; }
    public int Quantity { get; private set; }

    /// <summary>Optional reference to the originating Order or ImportOrder.</summary>
    public Guid? ReferenceId { get; private set; }

    private InventoryTransaction() { } // ORM / serialization

    // ──────────────────────────────────────────────
    //  Factory
    // ──────────────────────────────────────────────

    /// <summary>Records a single stock movement event.</summary>
    public static InventoryTransaction Create(
        Guid productId,
        Guid warehouseId,
        InventoryTransactionType type,
        int quantity,
        Guid? referenceId = null)
    {
        if (productId == Guid.Empty)
            throw new DomainException("ProductId cannot be empty.");

        if (warehouseId == Guid.Empty)
            throw new DomainException("WarehouseId cannot be empty.");

        if (quantity == 0)
            throw new DomainException("Transaction quantity cannot be zero.");

        return new InventoryTransaction
        {
            ProductId = productId,
            WarehouseId = warehouseId,
            Type = type,
            Quantity = quantity,
            ReferenceId = referenceId
        };
    }
}
