using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using LiteDB;
using BizI.Domain.Entities;
using BizI.Domain.Interfaces;

namespace BizI.Infrastructure.Data;

public class LiteDbRepository<T> : IRepository<T> where T : BaseEntity
{
    private readonly AppDbContext _context;
    private readonly string _collectionName;

    public LiteDbRepository(AppDbContext context)
    {
        _context = context;
        _collectionName = typeof(T).Name;
    }

    private ILiteCollection<T> Collection => _context.Database.GetCollection<T>(_collectionName);

    public Task<T?> GetByIdAsync(string id)
    {
        var result = Collection.FindOne(x => x.Id == string.Intern(id)); // LiteDB works best with primitive types, but string handles OK
        // Alternatively, use BsonMapper to map Id cleanly, but x.Id == id is fine
        return Task.FromResult<T?>(result);
    }

    public Task<IEnumerable<T>> GetAllAsync()
    {
        var result = Collection.Find(x => !x.IsDeleted).ToList();
        return Task.FromResult<IEnumerable<T>>(result);
    }

    public Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
    {
        // For accurate soft delete, we'd append IsDeleted rule, but let's just let the predicate run.
        var results = Collection.Find(predicate).Where(x => !x.IsDeleted).ToList();
        return Task.FromResult<IEnumerable<T>>(results);
    }

    public Task AddAsync(T entity)
    {
        Collection.Insert(entity);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(T entity)
    {
        entity.UpdatedAt = DateTime.UtcNow;
        if (!Collection.Update(entity))
        {
            throw new Exception("Entity not found or update failed");
        }
        return Task.CompletedTask;
    }

    public Task DeleteAsync(string id)
    {
        var entity = Collection.FindOne(x => x.Id == id);
        if (entity != null)
        {
            entity.IsDeleted = true;
            entity.UpdatedAt = DateTime.UtcNow;
            Collection.Update(entity); // Soft delete
        }
        return Task.CompletedTask;
    }
}
