namespace BizI.Application.Interfaces;

/// <summary>
/// Application-level service contract for all stock movement operations.
/// Uses domain repository interfaces only — no direct DB access.
/// </summary>
public interface IInventoryService
{
    /// <summary>Increases stock for a product in a warehouse.</summary>
    Task ImportStockAsync(Guid productId, Guid warehouseId, int quantity, Guid? referenceId = null);

    /// <summary>Decreases stock; throws InsufficientStockException if not enough stock.</summary>
    Task ExportStockAsync(Guid productId, Guid warehouseId, int quantity, Guid? referenceId = null);

    /// <summary>Restores stock (e.g. customer return).</summary>
    Task ReturnStockAsync(Guid productId, Guid warehouseId, int quantity, Guid? referenceId = null);

    /// <summary>Sets stock to an absolute value (physical count adjustment).</summary>
    Task AdjustStockAsync(Guid productId, Guid warehouseId, int newQuantity);
}
