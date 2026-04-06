using System.Linq.Expressions;
using BizI.Domain.Entities;
using BizI.Domain.Interfaces;
using BizI.Infrastructure.Persistence.LiteDb;
using Microsoft.Extensions.Logging;

namespace BizI.Infrastructure.Persistence.Repositories;

/// <summary>
/// Generic LiteDB repository providing standard CRUD operations.
/// Implements soft-delete pattern using the <see cref="BaseEntity.IsDeleted"/> flag.
/// </summary>
public class LiteDbRepository<T> : IRepository<T> where T : BaseEntity
{
    private readonly ILiteDbContext _context;
    private readonly ILogger<LiteDbRepository<T>> _logger;
    private readonly string _collectionName;

    public LiteDbRepository(ILiteDbContext context, ILogger<LiteDbRepository<T>> logger)
    {
        _context = context;
        _logger = logger;
        _collectionName = typeof(T).Name;
    }

    protected virtual string CollectionName => _collectionName;

    protected LiteDB.ILiteCollection<T> Collection =>
        _context.GetCollection<T>(CollectionName);

    /// <inheritdoc />
    public Task<T?> GetByIdAsync(string id)
    {
        _logger.LogDebug("Getting {Entity} by id: {Id}", typeof(T).Name, id);
        var result = Collection.FindOne(x => x.Id == id && !x.IsDeleted);
        return Task.FromResult<T?>(result);
    }

    /// <inheritdoc />
    public Task<IEnumerable<T>> GetAllAsync()
    {
        _logger.LogDebug("Getting all {Entity} records.", typeof(T).Name);
        var result = Collection.Find(x => !x.IsDeleted).ToList();
        return Task.FromResult<IEnumerable<T>>(result);
    }

    /// <inheritdoc />
    public Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
    {
        var results = Collection.Find(predicate).Where(x => !x.IsDeleted).ToList();
        return Task.FromResult<IEnumerable<T>>(results);
    }

    /// <inheritdoc />
    public Task AddAsync(T entity)
    {
        _logger.LogDebug("Inserting {Entity} with id: {Id}", typeof(T).Name, entity.Id);
        Collection.Insert(entity);
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public Task UpdateAsync(T entity)
    {
        // Domain methods (Complete(), Cancel(), Return(), etc.) already call Touch() internally.
        // The repository must NOT call Touch() directly — Touch() is protected by design.
        _logger.LogDebug("Updating {Entity} with id: {Id}", typeof(T).Name, entity.Id);
        if (!Collection.Update(entity))
            throw new InvalidOperationException($"{typeof(T).Name} with id '{entity.Id}' was not found or the update failed.");

        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public Task DeleteAsync(string id)
    {
        _logger.LogDebug("Soft-deleting {Entity} with id: {Id}", typeof(T).Name, id);
        var entity = Collection.FindOne(x => x.Id == id);
        if (entity is null)
        {
            _logger.LogWarning("{Entity} with id '{Id}' not found for deletion.", typeof(T).Name, id);
            return Task.CompletedTask;
        }

        entity.MarkAsDeleted();
        Collection.Update(entity);
        return Task.CompletedTask;
    }
}
