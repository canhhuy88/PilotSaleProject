using BizI.Domain.Exceptions;
using BizI.Domain.ValueObjects;

namespace BizI.Domain.Entities;

/// <summary>
/// Child entity of the Product aggregate.
/// Represents a specific variant (e.g. size/colour combination) of a product.
/// </summary>
public class ProductVariant : BaseEntity
{
    public Guid ProductId { get; private set; }

    /// <summary>
    /// Attribute bag (e.g. {"Color": "Red", "Size": "M"}).
    /// Exposed as read-only; mutations go through domain methods.
    /// </summary>
    private readonly Dictionary<string, string> _attributes = new();
    public IReadOnlyDictionary<string, string> Attributes => _attributes;

    public string SKU { get; private set; } = string.Empty;
    public string? Barcode { get; private set; }

    /// <summary>Variant-specific selling price override.</summary>
    public Money Price { get; private set; } = Money.Zero;

    private ProductVariant() { } // ORM / serialization

    // ──────────────────────────────────────────────
    //  Factory
    // ──────────────────────────────────────────────

    /// <summary>Creates a valid ProductVariant linked to an existing Product.</summary>
    public static ProductVariant Create(
        Guid productId,
        string sku,
        decimal price,
        Dictionary<string, string>? attributes = null,
        string? barcode = null,
        string currency = "VND")
    {
        if (productId == Guid.Empty)
            throw new DomainException("ProductId cannot be empty.");

        if (string.IsNullOrWhiteSpace(sku))
            throw new DomainException("Variant SKU cannot be empty.");

        var variant = new ProductVariant
        {
            ProductId = productId,
            SKU = sku.Trim().ToUpperInvariant(),
            Price = new Money(price, currency),
            Barcode = barcode?.Trim()
        };

        if (attributes is not null)
            foreach (var kv in attributes)
                variant._attributes[kv.Key] = kv.Value;

        return variant;
    }

    // ──────────────────────────────────────────────
    //  Domain Methods
    // ──────────────────────────────────────────────

    /// <summary>Updates the variant's price.</summary>
    public void SetPrice(Money newPrice)
    {
        Price = newPrice;
        Touch();
    }

    /// <summary>Sets or updates a single attribute.</summary>
    public void SetAttribute(string key, string value)
    {
        if (string.IsNullOrWhiteSpace(key))
            throw new DomainException("Attribute key cannot be empty.");

        _attributes[key] = value;
        Touch();
    }

    /// <summary>Removes an attribute by key.</summary>
    public void RemoveAttribute(string key)
    {
        if (_attributes.Remove(key))
            Touch();
    }

    /// <summary>Replaces the barcode.</summary>
    public void SetBarcode(string? barcode)
    {
        Barcode = barcode?.Trim();
        Touch();
    }
}
