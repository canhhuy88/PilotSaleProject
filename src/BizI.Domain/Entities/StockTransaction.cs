namespace BizI.Domain.Entities;

public enum StockTransactionType
{
    IMPORT, EXPORT, TRANSFER, RETURN_IN, RETURN_OUT, ADJUSTMENT
}

public class StockTransaction : BaseEntity
{
    public StockTransactionType Type { get; set; }
    public string RefId { get; set; } = string.Empty;
    public string ProductId { get; set; } = string.Empty;
    public string WarehouseId { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public int BeforeQty { get; set; }
    public int AfterQty { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
}