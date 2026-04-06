using BizI.Domain.Entities;

namespace BizI.Domain.Interfaces;

/// <summary>
/// Specialized repository for Inventory aggregate.
/// Adds domain-specific query methods on top of the generic CRUD interface.
/// </summary>
public interface IInventoryRepository : IRepository<Inventory>
{
    /// <summary>Returns the inventory record for a specific product in a specific warehouse, or null if not found.</summary>
    Task<Inventory?> GetByProductAndWarehouseAsync(Guid productId, Guid warehouseId);

    /// <summary>Returns all inventory records for the given warehouse.</summary>
    Task<IEnumerable<Inventory>> GetByWarehouseAsync(Guid warehouseId);

    /// <summary>Returns all inventory records for the given product across all warehouses.</summary>
    Task<IEnumerable<Inventory>> GetByProductAsync(Guid productId);
}
