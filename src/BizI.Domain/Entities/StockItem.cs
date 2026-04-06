using BizI.Domain.Exceptions;
using BizI.Domain.ValueObjects;

namespace BizI.Domain.Entities;

/// <summary>
/// Represents real-time stock availability for a product in a specific warehouse.
/// Tracks both physical quantity and reservations.
/// </summary>
public class StockItem : BaseEntity
{
    public string ProductId { get; private set; } = string.Empty;
    public string WarehouseId { get; private set; } = string.Empty;

    /// <summary>Total physical units on hand.</summary>
    public int Quantity { get; private set; }

    /// <summary>Units reserved for pending orders (not yet dispatched).</summary>
    public int ReservedQty { get; private set; }

    /// <summary>Units freely available for new orders.</summary>
    public int AvailableQty => Quantity - ReservedQty;

    private StockItem() { } // ORM / serialization

    // ──────────────────────────────────────────────
    //  Factory
    // ──────────────────────────────────────────────

    public static StockItem Create(string productId, string warehouseId, int quantity = 0)
    {
        if (string.IsNullOrWhiteSpace(productId))
            throw new DomainException("ProductId cannot be empty.");

        if (string.IsNullOrWhiteSpace(warehouseId))
            throw new DomainException("WarehouseId cannot be empty.");

        if (quantity < 0)
            throw new DomainException("Stock quantity cannot be negative.");

        return new StockItem
        {
            ProductId = productId.Trim(),
            WarehouseId = warehouseId.Trim(),
            Quantity = quantity
        };
    }

    // ──────────────────────────────────────────────
    //  Domain Methods
    // ──────────────────────────────────────────────

    /// <summary>Adds physical stock (e.g. goods received).</summary>
    public void Receive(int qty)
    {
        if (qty <= 0)
            throw new DomainException("Receive quantity must be positive.");

        Quantity += qty;
        Touch();
    }

    /// <summary>Removes physical stock (e.g. dispatch or write-off).</summary>
    public void Dispatch(int qty)
    {
        if (qty <= 0)
            throw new DomainException("Dispatch quantity must be positive.");

        if (qty > AvailableQty)
            throw new InsufficientStockException(ProductId, AvailableQty, qty);

        Quantity -= qty;
        Touch();
    }

    /// <summary>Reserves units for a pending order.</summary>
    public void Reserve(int qty)
    {
        if (qty <= 0)
            throw new DomainException("Reserve quantity must be positive.");

        if (qty > AvailableQty)
            throw new DomainException($"Cannot reserve {qty} units — only {AvailableQty} available.");

        ReservedQty += qty;
        Touch();
    }

    /// <summary>Releases a previous reservation (e.g. order cancelled).</summary>
    public void ReleaseReservation(int qty)
    {
        if (qty <= 0)
            throw new DomainException("Release quantity must be positive.");

        ReservedQty = Math.Max(0, ReservedQty - qty);
        Touch();
    }
}
