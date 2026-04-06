using BizI.Domain.Exceptions;

namespace BizI.Domain.Entities;

/// <summary>
/// Enumeration of all recognised stock movement directions.
/// Defined alongside the entity it belongs to (no separate Enums file needed).
/// </summary>
public enum StockTransactionType
{
    Import,
    Export,
    Transfer,
    ReturnIn,
    ReturnOut,
    Adjustment
}

/// <summary>
/// Immutable audit record of a single stock movement.
/// Captures before/after quantities to enable full ledger reconstruction.
/// </summary>
public class StockTransaction : BaseEntity
{
    public StockTransactionType Type { get; private set; }

    /// <summary>ID of the source document (Order, StockIn, etc.).</summary>
    public string RefId { get; private set; } = string.Empty;

    public string ProductId { get; private set; } = string.Empty;
    public string WarehouseId { get; private set; } = string.Empty;

    /// <summary>Units moved (always positive; direction implied by Type).</summary>
    public int Quantity { get; private set; }

    public int BeforeQty { get; private set; }
    public int AfterQty { get; private set; }
    public string CreatedBy { get; private set; } = string.Empty;

    private StockTransaction() { } // ORM / serialization

    // ──────────────────────────────────────────────
    //  Factory
    // ──────────────────────────────────────────────

    /// <summary>
    /// Records a stock ledger entry. BeforeQty and AfterQty provide a full audit trail.
    /// </summary>
    public static StockTransaction Create(
        StockTransactionType type,
        string productId,
        string warehouseId,
        int quantity,
        int beforeQty,
        int afterQty,
        string refId = "",
        string createdBy = "")
    {
        if (string.IsNullOrWhiteSpace(productId))
            throw new DomainException("ProductId cannot be empty.");

        if (string.IsNullOrWhiteSpace(warehouseId))
            throw new DomainException("WarehouseId cannot be empty.");

        if (quantity == 0)
            throw new DomainException("Transaction quantity cannot be zero.");

        if (beforeQty < 0)
            throw new DomainException("BeforeQty cannot be negative.");

        return new StockTransaction
        {
            Type = type,
            ProductId = productId.Trim(),
            WarehouseId = warehouseId.Trim(),
            Quantity = quantity,
            BeforeQty = beforeQty,
            AfterQty = afterQty,
            RefId = refId.Trim(),
            CreatedBy = createdBy.Trim()
        };
    }
}
