using BizI.Domain.Exceptions;

namespace BizI.Domain.Entities;

/// <summary>
/// Represents the stock level of a specific product in a specific warehouse.
/// Aggregate Root for stock quantity management.
/// </summary>
public class Inventory : BaseEntity
{
    public Guid ProductId { get; private set; }
    public Guid WarehouseId { get; private set; }
    public int Quantity { get; private set; }

    private Inventory() { } // ORM / serialization

    // ──────────────────────────────────────────────
    //  Factory
    // ──────────────────────────────────────────────

    /// <summary>Creates an Inventory record for a product/warehouse pair.</summary>
    public static Inventory Create(Guid productId, Guid warehouseId, int initialQuantity = 0)
    {
        if (productId == Guid.Empty)
            throw new DomainException("ProductId cannot be empty.");

        if (warehouseId == Guid.Empty)
            throw new DomainException("WarehouseId cannot be empty.");

        if (initialQuantity < 0)
            throw new DomainException("Initial inventory quantity cannot be negative.");

        return new Inventory
        {
            ProductId = productId,
            WarehouseId = warehouseId,
            Quantity = initialQuantity
        };
    }

    // ──────────────────────────────────────────────
    //  Domain Methods
    // ──────────────────────────────────────────────

    /// <summary>Increases stock (e.g. goods received from supplier).</summary>
    public void AddStock(int quantity)
    {
        if (quantity <= 0)
            throw new DomainException("Import quantity must be greater than zero.");

        Quantity += quantity;
        Touch();
    }

    /// <summary>
    /// Decreases stock (e.g. sale or write-off).
    /// Throws <see cref="BizI.Domain.Exceptions.InsufficientStockException"/> when requested quantity exceeds available stock.
    /// </summary>
    public void DeductStock(int quantity)
    {
        if (quantity <= 0)
            throw new DomainException("Export quantity must be greater than zero.");

        if (Quantity < quantity)
            throw new InsufficientStockException(ProductId, Quantity, quantity);

        Quantity -= quantity;
        Touch();
    }

    /// <summary>
    /// Sets the stock quantity to an absolute value (physical count adjustment).
    /// Returns the delta (positive = gain, negative = loss).
    /// </summary>
    public int SetQuantity(int newQuantity)
    {
        if (newQuantity < 0)
            throw new DomainException("Inventory quantity cannot be negative.");

        int delta = newQuantity - Quantity;
        Quantity = newQuantity;
        Touch();
        return delta;
    }
}
