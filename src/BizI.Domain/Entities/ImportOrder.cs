using System.Collections.Generic;

namespace BizI.Domain.Entities;

public class ImportOrder : BaseEntity
{
    public string SupplierId { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public string Status { get; set; } = string.Empty;
    public List<ImportOrderItem> Items { get; set; } = new();
}
public class ImportOrderItem { public string ProductId { get; set; } = string.Empty; public int Quantity { get; set; } public decimal CostPrice { get; set; } }