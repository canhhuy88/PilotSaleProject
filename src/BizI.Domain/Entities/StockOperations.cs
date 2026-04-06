using System.Collections.Generic;

namespace BizI.Domain.Entities;

public class StockIn : BaseEntity
{
    public string Code { get; set; } = string.Empty;
    public string SupplierId { get; set; } = string.Empty;
    public string WarehouseId { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public List<StockInItem> Items { get; set; } = new();
}
public class StockInItem { public string ProductId { get; set; } = string.Empty; public int Quantity { get; set; } public decimal CostPrice { get; set; } }

public class StockOut : BaseEntity
{
    public string Code { get; set; } = string.Empty;
    public string WarehouseId { get; set; } = string.Empty;
    public string Reason { get; set; } = string.Empty;
    public List<StockOutItem> Items { get; set; } = new();
}
public class StockOutItem { public string ProductId { get; set; } = string.Empty; public int Quantity { get; set; } }

public class StockTransfer : BaseEntity
{
    public string FromWarehouseId { get; set; } = string.Empty;
    public string ToWarehouseId { get; set; } = string.Empty;
    public List<StockTransferItem> Items { get; set; } = new();
}
public class StockTransferItem { public string ProductId { get; set; } = string.Empty; public int Quantity { get; set; } }

public class StockAudit : BaseEntity
{
    public string WarehouseId { get; set; } = string.Empty;
    public List<StockAuditItem> Items { get; set; } = new();
}
public class StockAuditItem { public string ProductId { get; set; } = string.Empty; public int SystemQty { get; set; } public int ActualQty { get; set; } public int Difference { get; set; } }