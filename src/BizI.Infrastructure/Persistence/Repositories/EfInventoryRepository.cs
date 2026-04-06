using BizI.Domain.Entities;
using BizI.Domain.Interfaces;
using BizI.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BizI.Infrastructure.Persistence.Repositories;

/// <summary>
/// EF Core implementation of <see cref="IInventoryRepository"/>.
/// Note: Domain entity IDs are stored as strings (Guid.ToString("N")).
/// Guid parameters are converted to string for EF LINQ queries.
/// </summary>
public class EfInventoryRepository : EfRepository<Inventory>, IInventoryRepository
{
    public EfInventoryRepository(AppDbContext context, ILogger<EfInventoryRepository> logger)
        : base(context, logger) { }

    /// <inheritdoc />
    public async Task<Inventory?> GetByProductAndWarehouseAsync(Guid productId, Guid warehouseId)
    {
        var productIdStr = productId.ToString();
        var warehouseIdStr = warehouseId.ToString();

        return await DbSet.FirstOrDefaultAsync(x =>
            x.ProductId == productIdStr &&
            x.WarehouseId == warehouseIdStr &&
            !x.IsDeleted);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Inventory>> GetByWarehouseAsync(Guid warehouseId)
    {
        var warehouseIdStr = warehouseId.ToString();
        return await DbSet
            .Where(x => x.WarehouseId == warehouseIdStr && !x.IsDeleted)
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Inventory>> GetByProductAsync(Guid productId)
    {
        var productIdStr = productId.ToString();
        return await DbSet
            .Where(x => x.ProductId == productIdStr && !x.IsDeleted)
            .ToListAsync();
    }
}
