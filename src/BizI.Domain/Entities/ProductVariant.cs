using System.Collections.Generic;

namespace BizI.Domain.Entities;

public class ProductVariant : BaseEntity
{
    public string ProductId { get; set; } = string.Empty;
    public Dictionary<string, string> Attributes { get; set; } = new();
    public string SKU { get; set; } = string.Empty;
    public string? Barcode { get; set; }
    public decimal Price { get; set; }
}