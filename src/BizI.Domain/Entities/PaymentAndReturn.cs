using BizI.Domain.Exceptions;
using BizI.Domain.ValueObjects;

namespace BizI.Domain.Entities;

/// <summary>
/// Represents a payment transaction recorded against a sales order.
/// </summary>
public class Payment : BaseEntity
{
    public Guid OrderId { get; private set; }

    /// <summary>Amount tendered in this payment.</summary>
    public Money Amount { get; private set; } = Money.Zero;

    /// <summary>Payment method (e.g. "Cash", "Card", "Transfer").</summary>
    public string Method { get; private set; } = string.Empty;

    private Payment() { } // ORM / serialization

    public static Payment Create(Guid orderId, decimal amount, string method, string currency = "VND")
    {
        if (orderId == Guid.Empty)
            throw new DomainException("OrderId cannot be empty.");

        if (string.IsNullOrWhiteSpace(method))
            throw new DomainException("Payment method cannot be empty.");

        return new Payment
        {
            OrderId = orderId,
            Amount = new Money(amount, currency),
            Method = method.Trim()
        };
    }
}

/// <summary>
/// Represents a customer's outstanding debt arising from a credit sale.
/// Tracks the total owed and how much has been repaid.
/// </summary>
public class Debt : BaseEntity
{
    public Guid CustomerId { get; private set; }
    public Guid OrderId { get; private set; }

    /// <summary>Total debt amount for this record.</summary>
    public Money Amount { get; private set; } = Money.Zero;

    /// <summary>Amount already repaid against this debt record.</summary>
    public Money PaidAmount { get; private set; } = Money.Zero;

    /// <summary>Status: "Unpaid" | "PartiallyPaid" | "Paid".</summary>
    public string Status { get; private set; } = "Unpaid";

    /// <summary>Remaining balance on this debt record.</summary>
    public Money RemainingAmount => new Money(
        Math.Max(0, Amount.Amount - PaidAmount.Amount), Amount.Currency);

    private Debt() { } // ORM / serialization

    public static Debt Create(Guid customerId, Guid orderId, decimal amount, string currency = "VND")
    {
        if (customerId == Guid.Empty)
            throw new DomainException("CustomerId cannot be empty.");

        if (orderId == Guid.Empty)
            throw new DomainException("OrderId cannot be empty.");

        return new Debt
        {
            CustomerId = customerId,
            OrderId = orderId,
            Amount = new Money(amount, currency)
        };
    }

    /// <summary>Records a repayment against this debt.</summary>
    public void RecordPayment(decimal paidAmount, string currency = "VND")
    {
        if (paidAmount <= 0)
            throw new DomainException("Payment amount must be positive.");

        var newPaid = PaidAmount.Amount + paidAmount;
        if (newPaid > Amount.Amount)
            throw new DomainException("Payment amount exceeds outstanding debt.");

        PaidAmount = new Money(newPaid, currency);
        Status = PaidAmount.Amount >= Amount.Amount ? "Paid" : "PartiallyPaid";
        Touch();
    }
}

/// <summary>
/// Aggregate Root: represents a return of goods from a customer against an existing order.
/// </summary>
public class ReturnOrder : BaseEntity
{
    public Guid OrderId { get; private set; }

    /// <summary>Total refund amount covering all returned lines.</summary>
    public Money TotalRefund { get; private set; } = Money.Zero;

    private readonly List<ReturnItem> _items = new();
    public IReadOnlyCollection<ReturnItem> Items => _items.AsReadOnly();

    private ReturnOrder() { } // ORM / serialization

    public static ReturnOrder Create(Guid orderId, IEnumerable<ReturnItem> items, string currency = "VND")
    {
        if (orderId == Guid.Empty)
            throw new DomainException("OrderId cannot be empty.");

        var itemList = items?.ToList() ?? new List<ReturnItem>();
        if (!itemList.Any())
            throw new DomainException("A return order must contain at least one item.");

        var returnOrder = new ReturnOrder { OrderId = orderId };
        foreach (var item in itemList)
            returnOrder._items.Add(item);

        var total = returnOrder._items.Sum(i => i.RefundPrice.Amount * i.Quantity);
        returnOrder.TotalRefund = new Money(total, currency);
        return returnOrder;
    }
}

/// <summary>
/// Value-object-like child of ReturnOrder representing a single returned product line.
/// </summary>
public class ReturnItem
{
    public Guid ProductId { get; private set; }
    public int Quantity { get; private set; }

    /// <summary>Per-unit refund price for this return line.</summary>
    public Money RefundPrice { get; private set; } = Money.Zero;

    /// <summary>Total refund for this line.</summary>
    public Money LineRefund => new Money(RefundPrice.Amount * Quantity, RefundPrice.Currency);

    private ReturnItem() { } // ORM

    public static ReturnItem Create(Guid productId, int quantity, decimal refundPrice, string currency = "VND")
    {
        if (productId == Guid.Empty)
            throw new DomainException("ProductId cannot be empty.");

        if (quantity <= 0)
            throw new DomainException("Return quantity must be positive.");

        return new ReturnItem
        {
            ProductId = productId,
            Quantity = quantity,
            RefundPrice = new Money(refundPrice, currency)
        };
    }
}
