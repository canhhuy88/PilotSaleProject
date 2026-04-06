using BizI.Domain.Exceptions;
using BizI.Domain.ValueObjects;

namespace BizI.Domain.Entities;

/// <summary>
/// Child entity of the Order aggregate.
/// Tracks the sold quantity, price, and partial return state for a single product line.
/// </summary>
public class OrderItem
{
    public string ProductId { get; private set; } = string.Empty;
    public int Quantity { get; private set; }
    public int ReturnedQuantity { get; private set; }

    /// <summary>Unit selling price at time of sale (immutable after creation).</summary>
    public Money Price { get; private set; } = Money.Zero;

    // ──────────────────────────────────────────────
    //  Computed
    // ──────────────────────────────────────────────

    public int RemainingQuantity => Quantity - ReturnedQuantity;
    public Money LineTotal => new Money(Price.Amount * Quantity, Price.Currency);

    private OrderItem() { } // ORM / serialization

    // ──────────────────────────────────────────────
    //  Factory
    // ──────────────────────────────────────────────

    /// <summary>Creates a valid order line item. Price must be non-negative.</summary>
    public static OrderItem Create(string productId, int quantity, decimal price, string currency = "VND")
    {
        if (string.IsNullOrWhiteSpace(productId))
            throw new DomainException("ProductId cannot be empty.");

        if (quantity <= 0)
            throw new DomainException("Order item quantity must be greater than zero.");

        return new OrderItem
        {
            ProductId = productId.Trim(),
            Quantity = quantity,
            Price = new Money(price, currency)  // Money validates price >= 0
        };
    }

    // ──────────────────────────────────────────────
    //  Domain Methods
    // ──────────────────────────────────────────────

    /// <summary>
    /// Records a partial or full return of this line item.
    /// Throws if the requested return quantity exceeds the remaining returnable quantity.
    /// </summary>
    public void Return(int quantity)
    {
        if (quantity <= 0)
            throw new DomainException("Return quantity must be greater than zero.");

        if (ReturnedQuantity + quantity > Quantity)
            throw new DomainException(
                $"Cannot return {quantity} unit(s). Only {RemainingQuantity} unit(s) available for return.");

        ReturnedQuantity += quantity;
    }
}
