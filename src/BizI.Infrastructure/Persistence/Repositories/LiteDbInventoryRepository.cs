using BizI.Domain.Entities;
using BizI.Domain.Interfaces;
using BizI.Infrastructure.Persistence.LiteDb;
using Microsoft.Extensions.Logging;

namespace BizI.Infrastructure.Persistence.Repositories;

/// <summary>
/// LiteDB implementation of <see cref="IInventoryRepository"/>.
/// Adds domain-specific inventory queries on top of the generic repository.
/// Note: Domain entity IDs are stored as strings; Guid parameters are converted to string for comparison.
/// </summary>
public class LiteDbInventoryRepository : LiteDbRepository<Inventory>, IInventoryRepository
{
    public LiteDbInventoryRepository(ILiteDbContext context, ILogger<LiteDbInventoryRepository> logger)
        : base(context, logger) { }

    protected override string CollectionName => CollectionNames.Inventory;

    /// <inheritdoc />
    public Task<Inventory?> GetByProductAndWarehouseAsync(Guid productId, Guid warehouseId)
    {
        // Domain entity stores IDs as strings — convert Guid to string for LiteDB LINQ query
        var productIdStr = productId.ToString();
        var warehouseIdStr = warehouseId.ToString();

        var result = Collection.FindOne(x =>
            x.ProductId == productIdStr &&
            x.WarehouseId == warehouseIdStr &&
            !x.IsDeleted);

        return Task.FromResult<Inventory?>(result);
    }

    /// <inheritdoc />
    public Task<IEnumerable<Inventory>> GetByWarehouseAsync(Guid warehouseId)
    {
        var warehouseIdStr = warehouseId.ToString();
        var results = Collection
            .Find(x => x.WarehouseId == warehouseIdStr && !x.IsDeleted)
            .ToList();

        return Task.FromResult<IEnumerable<Inventory>>(results);
    }

    /// <inheritdoc />
    public Task<IEnumerable<Inventory>> GetByProductAsync(Guid productId)
    {
        var productIdStr = productId.ToString();
        var results = Collection
            .Find(x => x.ProductId == productIdStr && !x.IsDeleted)
            .ToList();

        return Task.FromResult<IEnumerable<Inventory>>(results);
    }
}
