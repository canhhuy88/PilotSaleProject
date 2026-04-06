using System;

namespace BizI.Domain.Entities;

public class Inventory : BaseEntity
{
    public Guid ProductId { get; set; }
    public Guid WarehouseId { get; set; }
    public int Quantity { get; set; }
}
