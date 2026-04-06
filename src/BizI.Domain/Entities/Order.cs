using System.Collections.Generic;

namespace BizI.Domain.Entities;

public class Order : BaseEntity
{
    public string Code { get; set; } = string.Empty;
    public string CustomerId { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public decimal Discount { get; set; }
    public decimal FinalAmount { get; set; }
    public string Status { get; set; } = "Pending";
    public string PaymentStatus { get; set; } = "Unpaid";
    public string CreatedBy { get; set; } = string.Empty;
    public List<OrderItem> Items { get; set; } = new();
}


