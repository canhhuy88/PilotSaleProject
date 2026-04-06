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
        
        

        return await DbSet.FirstOrDefaultAsync(x =>
            x.ProductId == productId &&
            x.WarehouseId == warehouseId &&
            !x.IsDeleted);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Inventory>> GetByWarehouseAsync(Guid warehouseId)
    {
        
        return await DbSet
            .Where(x => x.WarehouseId == warehouseId && !x.IsDeleted)
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Inventory>> GetByProductAsync(Guid productId)
    {
        
        return await DbSet
            .Where(x => x.ProductId == productId && !x.IsDeleted)
            .ToListAsync();
    }
}
