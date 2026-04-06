namespace BizI.Application.Interfaces;

/// <summary>
/// Application-level service contract for all stock movement operations.
/// Uses domain repository interfaces only — no direct DB access.
/// </summary>
public interface IInventoryService
{
    /// <summary>Increases stock for a product in a warehouse.</summary>
    Task ImportStockAsync(string productId, string warehouseId, int quantity, string? referenceId = null);

    /// <summary>Decreases stock; throws InsufficientStockException if not enough stock.</summary>
    Task ExportStockAsync(string productId, string warehouseId, int quantity, string? referenceId = null);

    /// <summary>Restores stock (e.g. customer return).</summary>
    Task ReturnStockAsync(string productId, string warehouseId, int quantity, string? referenceId = null);

    /// <summary>Sets stock to an absolute value (physical count adjustment).</summary>
    Task AdjustStockAsync(string productId, string warehouseId, int newQuantity);
}
