using BizI.Domain.Exceptions;
using BizI.Domain.ValueObjects;

namespace BizI.Domain.Entities;

/// <summary>
/// Aggregate Root: represents a supplier import / purchase order.
/// Tracks goods received with quantities and cost prices.
/// </summary>
public class ImportOrder : BaseEntity
{
    public string SupplierId { get; private set; } = string.Empty;

    /// <summary>Calculated total cost for all items in this order.</summary>
    public Money TotalAmount { get; private set; } = Money.Zero;

    /// <summary>Lifecycle status (e.g. "Draft", "Confirmed", "Received", "Cancelled").</summary>
    public string Status { get; private set; } = "Draft";

    private readonly List<ImportOrderItem> _items = new();
    public IReadOnlyCollection<ImportOrderItem> Items => _items.AsReadOnly();

    private ImportOrder() { } // ORM / serialization

    // ──────────────────────────────────────────────
    //  Factory
    // ──────────────────────────────────────────────

    /// <summary>Creates a new import order in Draft status.</summary>
    public static ImportOrder Create(string supplierId, IEnumerable<ImportOrderItem>? items = null)
    {
        if (string.IsNullOrWhiteSpace(supplierId))
            throw new DomainException("SupplierId cannot be empty.");

        var order = new ImportOrder { SupplierId = supplierId.Trim() };

        if (items is not null)
            foreach (var item in items)
                order.AddItem(item);

        return order;
    }

    // ──────────────────────────────────────────────
    //  Domain Methods
    // ──────────────────────────────────────────────

    /// <summary>Appends a new line item to this import order.</summary>
    public void AddItem(ImportOrderItem item)
    {
        if (Status != "Draft")
            throw new DomainException("Items can only be added to Draft import orders.");

        _items.Add(item);
        RecalculateTotal();
    }

    /// <summary>Confirms the order (locked for further edits).</summary>
    public void Confirm()
    {
        if (Status != "Draft")
            throw new DomainException("Only Draft import orders can be confirmed.");

        if (!_items.Any())
            throw new DomainException("Cannot confirm an import order with no items.");

        Status = "Confirmed";
        Touch();
    }

    /// <summary>Marks the order as received (stock has been physically received).</summary>
    public void MarkReceived()
    {
        if (Status != "Confirmed")
            throw new DomainException("Only Confirmed import orders can be marked as received.");

        Status = "Received";
        Touch();
    }

    /// <summary>Cancels the import order.</summary>
    public void Cancel()
    {
        if (Status == "Received")
            throw new DomainException("Cannot cancel an already received import order.");

        Status = "Cancelled";
        Touch();
    }

    // ──────────────────────────────────────────────
    //  Private Helpers
    // ──────────────────────────────────────────────

    private void RecalculateTotal()
    {
        var total = _items.Sum(i => i.CostPrice.Amount * i.Quantity);
        TotalAmount = new Money(total, TotalAmount.Currency);
    }
}

/// <summary>
/// Value-object-like child of ImportOrder.
/// Holds the product, quantity, and cost price for a single import line.
/// </summary>
public class ImportOrderItem
{
    public string ProductId { get; private set; } = string.Empty;
    public int Quantity { get; private set; }

    /// <summary>Unit cost paid to the supplier for this line.</summary>
    public Money CostPrice { get; private set; } = Money.Zero;

    private ImportOrderItem() { } // ORM

    public static ImportOrderItem Create(string productId, int quantity, decimal costPrice, string currency = "VND")
    {
        if (string.IsNullOrWhiteSpace(productId))
            throw new DomainException("ProductId cannot be empty.");

        if (quantity <= 0)
            throw new DomainException("Import item quantity must be greater than zero.");

        return new ImportOrderItem
        {
            ProductId = productId.Trim(),
            Quantity = quantity,
            CostPrice = new Money(costPrice, currency)
        };
    }
}
