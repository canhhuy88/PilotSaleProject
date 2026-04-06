using System;

namespace BizI.Domain.Entities;

public class OrderItem
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; } // Originally sold qty
    public int ReturnedQuantity { get; set; } // Track returns
    public decimal Price { get; set; }
}
