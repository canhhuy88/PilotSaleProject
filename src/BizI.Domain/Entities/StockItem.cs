namespace BizI.Domain.Entities;

public class StockItem : BaseEntity
{
    public string ProductId { get; set; } = string.Empty;
    public string WarehouseId { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public int ReservedQty { get; set; }
}