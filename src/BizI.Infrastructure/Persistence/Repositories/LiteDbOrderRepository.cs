using BizI.Domain.Entities;
using BizI.Domain.Enums;
using BizI.Domain.Interfaces;
using BizI.Infrastructure.Persistence.LiteDb;
using Microsoft.Extensions.Logging;

namespace BizI.Infrastructure.Persistence.Repositories;

/// <summary>
/// LiteDB implementation of <see cref="IOrderRepository"/>.
/// </summary>
public class LiteDbOrderRepository : LiteDbRepository<Order>, IOrderRepository
{
    public LiteDbOrderRepository(ILiteDbContext context, ILogger<LiteDbOrderRepository> logger)
        : base(context, logger) { }

    protected override string CollectionName => CollectionNames.Orders;

    /// <inheritdoc />
    public Task<Order?> GetByCodeAsync(string code)
    {
        var result = Collection.FindOne(x => x.Code == code && !x.IsDeleted);
        return Task.FromResult<Order?>(result);
    }

    /// <inheritdoc />
    public Task<IEnumerable<Order>> GetByCustomerAsync(string customerId)
    {
        var results = Collection
            .Find(x => x.CustomerId == customerId && !x.IsDeleted)
            .ToList();

        return Task.FromResult<IEnumerable<Order>>(results);
    }

    /// <inheritdoc />
    public Task<IEnumerable<Order>> GetByStatusAsync(OrderStatus status)
    {
        var statusStr = status.ToString();
        var results = Collection
            .Find(x => x.Status == status && !x.IsDeleted)
            .ToList();

        return Task.FromResult<IEnumerable<Order>>(results);
    }
}
