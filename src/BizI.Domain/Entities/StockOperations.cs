using BizI.Domain.Exceptions;
using BizI.Domain.ValueObjects;

namespace BizI.Domain.Entities;

/// <summary>
/// Aggregate Root: represents a goods-in operation from a supplier to a warehouse.
/// </summary>
public class StockIn : BaseEntity
{
    public string Code { get; private set; } = string.Empty;
    public string SupplierId { get; private set; } = string.Empty;
    public string WarehouseId { get; private set; } = string.Empty;

    /// <summary>Total cost of all received items.</summary>
    public Money TotalAmount { get; private set; } = Money.Zero;

    private readonly List<StockInItem> _items = new();
    public IReadOnlyCollection<StockInItem> Items => _items.AsReadOnly();

    private StockIn() { } // ORM / serialization

    public static StockIn Create(
        string code,
        string supplierId,
        string warehouseId,
        IEnumerable<StockInItem> items,
        string currency = "VND")
    {
        if (string.IsNullOrWhiteSpace(code))
            throw new DomainException("StockIn code cannot be empty.");

        if (string.IsNullOrWhiteSpace(supplierId))
            throw new DomainException("SupplierId cannot be empty.");

        if (string.IsNullOrWhiteSpace(warehouseId))
            throw new DomainException("WarehouseId cannot be empty.");

        var itemList = items?.ToList() ?? new List<StockInItem>();
        if (!itemList.Any())
            throw new DomainException("A StockIn must have at least one item.");

        var stockIn = new StockIn
        {
            Code = code.Trim(),
            SupplierId = supplierId.Trim(),
            WarehouseId = warehouseId.Trim()
        };

        foreach (var item in itemList) stockIn._items.Add(item);

        stockIn.TotalAmount = new Money(
            stockIn._items.Sum(i => i.CostPrice.Amount * i.Quantity), currency);

        return stockIn;
    }
}

/// <summary>
/// Child entity of StockIn: a single product line being received.
/// </summary>
public class StockInItem
{
    public string ProductId { get; private set; } = string.Empty;
    public int Quantity { get; private set; }
    public Money CostPrice { get; private set; } = Money.Zero;

    private StockInItem() { } // ORM

    public static StockInItem Create(string productId, int quantity, decimal costPrice, string currency = "VND")
    {
        if (string.IsNullOrWhiteSpace(productId))
            throw new DomainException("ProductId cannot be empty.");

        if (quantity <= 0)
            throw new DomainException("Quantity must be positive.");

        return new StockInItem
        {
            ProductId = productId.Trim(),
            Quantity = quantity,
            CostPrice = new Money(costPrice, currency)
        };
    }
}

// ─────────────────────────────────────────────────────────
//  StockOut
// ─────────────────────────────────────────────────────────

/// <summary>
/// Aggregate Root: represents a goods-out operation (write-off, write-down, etc.).
/// </summary>
public class StockOut : BaseEntity
{
    public string Code { get; private set; } = string.Empty;
    public string WarehouseId { get; private set; } = string.Empty;

    /// <summary>Business reason for the stock reduction (e.g. "Damaged", "Expired").</summary>
    public string Reason { get; private set; } = string.Empty;

    private readonly List<StockOutItem> _items = new();
    public IReadOnlyCollection<StockOutItem> Items => _items.AsReadOnly();

    private StockOut() { } // ORM / serialization

    public static StockOut Create(
        string code,
        string warehouseId,
        string reason,
        IEnumerable<StockOutItem> items)
    {
        if (string.IsNullOrWhiteSpace(code))
            throw new DomainException("StockOut code cannot be empty.");

        if (string.IsNullOrWhiteSpace(warehouseId))
            throw new DomainException("WarehouseId cannot be empty.");

        if (string.IsNullOrWhiteSpace(reason))
            throw new DomainException("A reason must be provided for a stock-out.");

        var itemList = items?.ToList() ?? new List<StockOutItem>();
        if (!itemList.Any())
            throw new DomainException("A StockOut must have at least one item.");

        var stockOut = new StockOut
        {
            Code = code.Trim(),
            WarehouseId = warehouseId.Trim(),
            Reason = reason.Trim()
        };

        foreach (var item in itemList) stockOut._items.Add(item);
        return stockOut;
    }
}

/// <summary>Child of StockOut.</summary>
public class StockOutItem
{
    public string ProductId { get; private set; } = string.Empty;
    public int Quantity { get; private set; }

    private StockOutItem() { } // ORM

    public static StockOutItem Create(string productId, int quantity)
    {
        if (string.IsNullOrWhiteSpace(productId))
            throw new DomainException("ProductId cannot be empty.");

        if (quantity <= 0)
            throw new DomainException("Quantity must be positive.");

        return new StockOutItem { ProductId = productId.Trim(), Quantity = quantity };
    }
}

// ─────────────────────────────────────────────────────────
//  StockTransfer
// ─────────────────────────────────────────────────────────

/// <summary>
/// Aggregate Root: represents an inter-warehouse stock transfer.
/// </summary>
public class StockTransfer : BaseEntity
{
    public string FromWarehouseId { get; private set; } = string.Empty;
    public string ToWarehouseId { get; private set; } = string.Empty;

    private readonly List<StockTransferItem> _items = new();
    public IReadOnlyCollection<StockTransferItem> Items => _items.AsReadOnly();

    private StockTransfer() { } // ORM / serialization

    public static StockTransfer Create(
        string fromWarehouseId,
        string toWarehouseId,
        IEnumerable<StockTransferItem> items)
    {
        if (string.IsNullOrWhiteSpace(fromWarehouseId))
            throw new DomainException("FromWarehouseId cannot be empty.");

        if (string.IsNullOrWhiteSpace(toWarehouseId))
            throw new DomainException("ToWarehouseId cannot be empty.");

        if (fromWarehouseId.Trim() == toWarehouseId.Trim())
            throw new DomainException("Source and destination warehouses must be different.");

        var itemList = items?.ToList() ?? new List<StockTransferItem>();
        if (!itemList.Any())
            throw new DomainException("A stock transfer must have at least one item.");

        var transfer = new StockTransfer
        {
            FromWarehouseId = fromWarehouseId.Trim(),
            ToWarehouseId = toWarehouseId.Trim()
        };

        foreach (var item in itemList) transfer._items.Add(item);
        return transfer;
    }
}

/// <summary>Child of StockTransfer.</summary>
public class StockTransferItem
{
    public string ProductId { get; private set; } = string.Empty;
    public int Quantity { get; private set; }

    private StockTransferItem() { } // ORM

    public static StockTransferItem Create(string productId, int quantity)
    {
        if (string.IsNullOrWhiteSpace(productId))
            throw new DomainException("ProductId cannot be empty.");

        if (quantity <= 0)
            throw new DomainException("Transfer quantity must be positive.");

        return new StockTransferItem { ProductId = productId.Trim(), Quantity = quantity };
    }
}

// ─────────────────────────────────────────────────────────
//  StockAudit
// ─────────────────────────────────────────────────────────

/// <summary>
/// Aggregate Root: represents a physical stock count audit for a warehouse.
/// Captures system quantity vs actual count and calculates discrepancies.
/// </summary>
public class StockAudit : BaseEntity
{
    public string WarehouseId { get; private set; } = string.Empty;

    private readonly List<StockAuditItem> _items = new();
    public IReadOnlyCollection<StockAuditItem> Items => _items.AsReadOnly();

    private StockAudit() { } // ORM / serialization

    public static StockAudit Create(string warehouseId, IEnumerable<StockAuditItem> items)
    {
        if (string.IsNullOrWhiteSpace(warehouseId))
            throw new DomainException("WarehouseId cannot be empty.");

        var itemList = items?.ToList() ?? new List<StockAuditItem>();
        if (!itemList.Any())
            throw new DomainException("A stock audit must include at least one item.");

        var audit = new StockAudit { WarehouseId = warehouseId.Trim() };
        foreach (var item in itemList) audit._items.Add(item);
        return audit;
    }
}

/// <summary>Child of StockAudit: captures the discrepancy for a single product.</summary>
public class StockAuditItem
{
    public string ProductId { get; private set; } = string.Empty;
    public int SystemQty { get; private set; }
    public int ActualQty { get; private set; }

    /// <summary>Discrepancy = Actual - System (positive = surplus, negative = loss).</summary>
    public int Difference => ActualQty - SystemQty;

    private StockAuditItem() { } // ORM

    public static StockAuditItem Create(string productId, int systemQty, int actualQty)
    {
        if (string.IsNullOrWhiteSpace(productId))
            throw new DomainException("ProductId cannot be empty.");

        if (systemQty < 0)
            throw new DomainException("System quantity cannot be negative.");

        if (actualQty < 0)
            throw new DomainException("Actual quantity cannot be negative.");

        return new StockAuditItem
        {
            ProductId = productId.Trim(),
            SystemQty = systemQty,
            ActualQty = actualQty
        };
    }
}
