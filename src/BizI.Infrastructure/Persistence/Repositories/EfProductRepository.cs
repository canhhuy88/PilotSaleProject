using BizI.Domain.Entities;
using BizI.Domain.Interfaces;
using BizI.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BizI.Infrastructure.Persistence.Repositories;

/// <summary>
/// EF Core implementation of <see cref="IProductRepository"/>.
/// </summary>
public class EfProductRepository : EfRepository<Product>, IProductRepository
{
    public EfProductRepository(AppDbContext context, ILogger<EfProductRepository> logger)
        : base(context, logger) { }

    /// <inheritdoc />
    public async Task<Product?> GetBySkuAsync(string sku)
    {
        return await DbSet.FirstOrDefaultAsync(x => x.SKU == sku && !x.IsDeleted);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Product>> GetByCategoryAsync(Guid categoryId)
    {
        return await DbSet
            .Where(x => x.CategoryId == categoryId && !x.IsDeleted)
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Product>> GetActiveProductsAsync()
    {
        return await DbSet
            .Where(x => x.IsActive && !x.IsDeleted)
            .ToListAsync();
    }
}
