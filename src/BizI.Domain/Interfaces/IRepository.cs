using System.Linq.Expressions;
using BizI.Domain.Entities;

namespace BizI.Domain.Interfaces;

/// <summary>
/// Generic repository interface providing common CRUD operations.
/// Implementations live in the Infrastructure layer only.
/// </summary>
public interface IRepository<T> where T : BaseEntity
{
    Task<T?> GetByIdAsync(Guid id);
    Task<IEnumerable<T>> GetAllAsync();
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(Guid id);
}
