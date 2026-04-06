using System;
using BizI.Domain.Enums;

namespace BizI.Domain.Entities;

public class InventoryTransaction : BaseEntity
{
    public Guid ProductId { get; set; }
    public Guid WarehouseId { get; set; }
    public InventoryTransactionType Type { get; set; }
    public int Quantity { get; set; }
    public new DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public Guid? ReferenceId { get; set; } // OrderId or ImportId
}
