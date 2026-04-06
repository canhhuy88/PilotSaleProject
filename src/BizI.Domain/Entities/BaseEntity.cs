namespace BizI.Domain.Entities;

/// <summary>
/// Base class for all domain entities.
/// Provides common identity, auditing, and soft-delete fields.
/// All setters are protected to allow ORM hydration while preventing
/// external mutation — only domain methods and constructors may set state.
/// </summary>
public abstract class BaseEntity
{
    public Guid Id { get; protected set; } = Guid.NewGuid();
    public DateTime CreatedAt { get; protected set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; protected set; }
    public bool IsDeleted { get; protected set; }

    /// <summary>Marks this entity as deleted (soft delete).</summary>
    public void MarkAsDeleted()
    {
        IsDeleted = true;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>Stamps the UpdatedAt field with the current UTC time.</summary>
    protected void Touch()
    {
        UpdatedAt = DateTime.UtcNow;
    }
}
