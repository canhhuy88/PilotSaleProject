namespace BizI.Domain.Entities;

public class Payment : BaseEntity
{
    public string OrderId { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Method { get; set; } = string.Empty;
}

public class Debt : BaseEntity
{
    public string CustomerId { get; set; } = string.Empty;
    public string OrderId { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public decimal PaidAmount { get; set; }
    public string Status { get; set; } = string.Empty;
}

public class ReturnOrder : BaseEntity
{
    public string OrderId { get; set; } = string.Empty;
    public decimal TotalRefund { get; set; }
    public System.Collections.Generic.List<ReturnItem> Items { get; set; } = new();
}
public class ReturnItem
{
    public string ProductId { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal RefundPrice { get; set; }
}