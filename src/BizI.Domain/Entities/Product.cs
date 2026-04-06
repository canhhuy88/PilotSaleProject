using BizI.Domain.Exceptions;
using BizI.Domain.ValueObjects;

namespace BizI.Domain.Entities;

/// <summary>
/// Aggregate Root: represents a sellable product.
/// Contains all business rules for pricing, activation, and margin calculation.
/// Child collection: ProductVariant (held by variant's ProductId foreign key).
/// </summary>
public class Product : BaseEntity
{
    public string Name { get; private set; } = string.Empty;
    public string SKU { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public string? Barcode { get; private set; }
    public string CategoryId { get; private set; } = string.Empty;

    /// <summary>Landed cost per unit (what we pay to the supplier).</summary>
    public Money CostPrice { get; private set; } = Money.Zero;

    /// <summary>Standard selling price per unit.</summary>
    public Money SalePrice { get; private set; } = Money.Zero;

    /// <summary>Unit of measurement (e.g. "piece", "kg", "box").</summary>
    public string Unit { get; private set; } = string.Empty;

    public bool IsActive { get; private set; } = true;

    private Product() { } // ORM / serialization

    // ──────────────────────────────────────────────
    //  Factory
    // ──────────────────────────────────────────────

    /// <summary>Creates a valid, active Product. Throws on any constraint violation.</summary>
    public static Product Create(
        string name,
        string sku,
        decimal costPrice,
        decimal salePrice,
        string unit,
        string categoryId,
        string? description = null,
        string? barcode = null,
        string currency = "VND")
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Product name cannot be empty.");

        if (string.IsNullOrWhiteSpace(sku))
            throw new DomainException("Product SKU cannot be empty.");

        if (string.IsNullOrWhiteSpace(unit))
            throw new DomainException("Product unit cannot be empty.");

        return new Product
        {
            Name = name.Trim(),
            SKU = sku.Trim().ToUpperInvariant(),
            CostPrice = new Money(costPrice, currency),
            SalePrice = new Money(salePrice, currency),
            Unit = unit.Trim(),
            CategoryId = categoryId.Trim(),
            Description = description?.Trim(),
            Barcode = barcode?.Trim()
        };
    }

    // ──────────────────────────────────────────────
    //  Domain Methods
    // ──────────────────────────────────────────────

    /// <summary>Updates all editable product details atomically.</summary>
    public void UpdateDetails(
        string name,
        string sku,
        decimal costPrice,
        decimal salePrice,
        string unit,
        string categoryId,
        string? description = null,
        string? barcode = null,
        string currency = "VND")
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Product name cannot be empty.");

        if (string.IsNullOrWhiteSpace(sku))
            throw new DomainException("Product SKU cannot be empty.");

        if (string.IsNullOrWhiteSpace(unit))
            throw new DomainException("Product unit cannot be empty.");

        Name = name.Trim();
        SKU = sku.Trim().ToUpperInvariant();
        CostPrice = new Money(costPrice, currency);
        SalePrice = new Money(salePrice, currency);
        Unit = unit.Trim();
        CategoryId = categoryId.Trim();
        Description = description?.Trim();
        Barcode = barcode?.Trim();
        Touch();
    }

    /// <summary>
    /// Adjusts the sale price. The new price must be non-negative.
    /// </summary>
    public void SetSalePrice(Money newPrice)
    {
        if (newPrice.Currency != CostPrice.Currency)
            throw new DomainException($"Currency mismatch: cannot set sale price in '{newPrice.Currency}' when cost is in '{CostPrice.Currency}'.");

        SalePrice = newPrice;
        Touch();
    }

    /// <summary>Adjusts the cost price (e.g. after a supplier price change).</summary>
    public void SetCostPrice(Money newCost)
    {
        if (newCost.Currency != SalePrice.Currency)
            throw new DomainException($"Currency mismatch: cannot set cost price in '{newCost.Currency}' when sale is in '{SalePrice.Currency}'.");

        CostPrice = newCost;
        Touch();
    }

    /// <summary>Activates this product so it can be sold.</summary>
    public void Activate()
    {
        IsActive = true;
        Touch();
    }

    /// <summary>Deactivates this product (hidden from sale screens).</summary>
    public void Deactivate()
    {
        IsActive = false;
        Touch();
    }

    // ──────────────────────────────────────────────
    //  Computed Properties
    // ──────────────────────────────────────────────

    /// <summary>
    /// Gross margin percentage: (SalePrice - CostPrice) / SalePrice × 100.
    /// Returns 0 when the sale price is zero.
    /// </summary>
    public decimal GrossMarginPercent =>
        SalePrice.IsZero
            ? 0
            : Math.Round((SalePrice.Amount - CostPrice.Amount) / SalePrice.Amount * 100, 2);
}
