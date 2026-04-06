using System.Linq.Expressions;
using BizI.Domain.Entities;
using BizI.Domain.Interfaces;
using BizI.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BizI.Infrastructure.Persistence.Repositories;

/// <summary>
/// Generic EF Core repository providing standard CRUD operations with soft-delete.
/// To switch databases: only change the provider in DI — no code changes here.
/// </summary>
public class EfRepository<T> : IRepository<T> where T : BaseEntity
{
    protected readonly AppDbContext Context;
    protected readonly DbSet<T> DbSet;
    private readonly ILogger<EfRepository<T>> _logger;

    public EfRepository(AppDbContext context, ILogger<EfRepository<T>> logger)
    {
        Context = context;
        DbSet = context.Set<T>();
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<T?> GetByIdAsync(Guid id)
    {
        _logger.LogDebug("Getting {Entity} by id: {Id}", typeof(T).Name, id);
        return await DbSet.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<T>> GetAllAsync()
    {
        _logger.LogDebug("Getting all {Entity} records.", typeof(T).Name);
        return await DbSet.Where(x => !x.IsDeleted).ToListAsync();
    }

    /// <inheritdoc />
    public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
    {
        return await DbSet.Where(predicate).Where(x => !x.IsDeleted).ToListAsync();
    }

    /// <inheritdoc />
    public async Task AddAsync(T entity)
    {
        _logger.LogDebug("Inserting {Entity} with id: {Id}", typeof(T).Name, entity.Id);
        await DbSet.AddAsync(entity);
        await Context.SaveChangesAsync();
    }

    /// <inheritdoc />
    public async Task UpdateAsync(T entity)
    {
        _logger.LogDebug("Updating {Entity} with id: {Id}", typeof(T).Name, entity.Id);
        DbSet.Update(entity);
        var affected = await Context.SaveChangesAsync();
        if (affected == 0)
            throw new InvalidOperationException(
                $"{typeof(T).Name} with id '{entity.Id}' was not found or the update failed.");
    }

    /// <inheritdoc />
    public async Task DeleteAsync(Guid id)
    {
        _logger.LogDebug("Soft-deleting {Entity} with id: {Id}", typeof(T).Name, id);
        var entity = await DbSet.FirstOrDefaultAsync(x => x.Id == id);
        if (entity is null)
        {
            _logger.LogWarning("{Entity} with id '{Id}' not found for deletion.", typeof(T).Name, id);
            return;
        }

        entity.MarkAsDeleted();
        await Context.SaveChangesAsync();
    }
}
