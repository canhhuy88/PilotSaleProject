using BizI.Domain.Entities;
using BizI.Domain.Enums;
using BizI.Domain.Interfaces;
using BizI.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BizI.Infrastructure.Persistence.Repositories;

/// <summary>
/// EF Core implementation of <see cref="IOrderRepository"/>.
/// </summary>
public class EfOrderRepository : EfRepository<Order>, IOrderRepository
{
    public EfOrderRepository(AppDbContext context, ILogger<EfOrderRepository> logger)
        : base(context, logger) { }

    /// <inheritdoc />
    public async Task<Order?> GetByCodeAsync(string code)
    {
        return await DbSet
            .Include("_items") // load owned collection via backing field
            .FirstOrDefaultAsync(x => x.Code == code && !x.IsDeleted);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Order>> GetByCustomerAsync(string customerId)
    {
        return await DbSet
            .Where(x => x.CustomerId == customerId && !x.IsDeleted)
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Order>> GetByStatusAsync(OrderStatus status)
    {
        return await DbSet
            .Where(x => x.Status == status && !x.IsDeleted)
            .ToListAsync();
    }
}
