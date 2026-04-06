using BizI.Domain.Enums;
using BizI.Domain.Exceptions;
using BizI.Domain.ValueObjects;

namespace BizI.Domain.Entities;

/// <summary>
/// Aggregate Root: represents a sales order including its line items.
/// Enforces business rules for status transitions, totals, and returns.
/// </summary>
public class Order : BaseEntity
{
    public string Code { get; private set; } = string.Empty;
    public string CustomerId { get; private set; } = string.Empty;

    /// <summary>Gross total before discount.</summary>
    public Money TotalAmount { get; private set; } = Money.Zero;

    /// <summary>Monetary discount applied to the order.</summary>
    public Money Discount { get; private set; } = Money.Zero;

    /// <summary>Net amount after discount (what the customer pays).</summary>
    public Money FinalAmount { get; private set; } = Money.Zero;

    public OrderStatus Status { get; private set; } = OrderStatus.Pending;
    public PaymentStatus PaymentStatus { get; private set; } = PaymentStatus.Unpaid;
    public string CreatedBy { get; private set; } = string.Empty;

    private readonly List<OrderItem> _items = new();
    public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();

    private Order() { } // ORM / serialization

    // ──────────────────────────────────────────────
    //  Factory
    // ──────────────────────────────────────────────

    /// <summary>
    /// Creates a valid Order with at least one line item.
    /// Totals are calculated automatically.
    /// </summary>
    public static Order Create(
        string code,
        string customerId,
        string createdBy,
        IEnumerable<OrderItem> items,
        decimal discount = 0m,
        string currency = "VND")
    {
        if (string.IsNullOrWhiteSpace(code))
            throw new DomainException("Order code cannot be empty.");

        var itemList = items?.ToList() ?? new List<OrderItem>();
        if (!itemList.Any())
            throw new DomainException("An order must contain at least one item.");

        var order = new Order
        {
            Code = code.Trim(),
            CustomerId = customerId.Trim(),
            CreatedBy = createdBy.Trim(),
            Discount = new Money(discount, currency)
        };

        foreach (var item in itemList)
            order._items.Add(item);

        order.RecalculateTotals(currency);
        return order;
    }

    // ──────────────────────────────────────────────
    //  Status Transitions
    // ──────────────────────────────────────────────

    /// <summary>Marks the order as completed and payment as paid.</summary>
    public void Complete()
    {
        if (Status != OrderStatus.Pending)
            throw new DomainException($"Cannot complete an order in '{Status}' status.");

        Status = OrderStatus.Completed;
        PaymentStatus = PaymentStatus.Paid;
        Touch();
    }

    /// <summary>Cancels the order (only if not already completed).</summary>
    public void Cancel()
    {
        if (Status == OrderStatus.Completed)
            throw new DomainException("Cannot cancel a completed order. Use a return instead.");

        Status = OrderStatus.Cancelled;
        Touch();
    }

    /// <summary>Marks the order as returned (only completed orders can be returned).</summary>
    public void MarkAsReturned()
    {
        if (Status != OrderStatus.Completed)
            throw new DomainException("Only completed orders can be returned.");

        Status = OrderStatus.Returned;
        Touch();
    }

    /// <summary>Updates the payment status independently (e.g. partial payment).</summary>
    public void MarkPaymentStatus(PaymentStatus paymentStatus)
    {
        PaymentStatus = paymentStatus;
        Touch();
    }

    // ──────────────────────────────────────────────
    //  Private Helpers
    // ──────────────────────────────────────────────

    private void RecalculateTotals(string currency = "VND")
    {
        decimal gross = _items.Sum(i => i.Price.Amount * i.Quantity);
        TotalAmount = new Money(gross, currency);
        decimal net = Math.Max(0m, gross - Discount.Amount);
        FinalAmount = new Money(net, currency);
    }
}
