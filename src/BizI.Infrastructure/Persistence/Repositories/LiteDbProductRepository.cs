using BizI.Domain.Entities;
using BizI.Domain.Interfaces;
using BizI.Infrastructure.Persistence.LiteDb;
using Microsoft.Extensions.Logging;

namespace BizI.Infrastructure.Persistence.Repositories;

/// <summary>
/// LiteDB implementation of <see cref="IProductRepository"/>.
/// </summary>
public class LiteDbProductRepository : LiteDbRepository<Product>, IProductRepository
{
    public LiteDbProductRepository(ILiteDbContext context, ILogger<LiteDbProductRepository> logger)
        : base(context, logger) { }

    protected override string CollectionName => CollectionNames.Products;

    /// <inheritdoc />
    public Task<Product?> GetBySkuAsync(string sku)
    {
        var result = Collection.FindOne(x => x.SKU == sku && !x.IsDeleted);
        return Task.FromResult<Product?>(result);
    }

    /// <inheritdoc />
    public Task<IEnumerable<Product>> GetByCategoryAsync(string categoryId)
    {
        var results = Collection
            .Find(x => x.CategoryId == categoryId && !x.IsDeleted)
            .ToList();

        return Task.FromResult<IEnumerable<Product>>(results);
    }

    /// <inheritdoc />
    public Task<IEnumerable<Product>> GetActiveProductsAsync()
    {
        var results = Collection
            .Find(x => x.IsActive && !x.IsDeleted)
            .ToList();

        return Task.FromResult<IEnumerable<Product>>(results);
    }
}
