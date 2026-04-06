using BizI.Domain.Exceptions;

namespace BizI.Domain.Entities;

/// <summary>
/// Represents a product category, optionally nested under a parent.
/// Follows DDD — all state changes go through domain methods.
/// </summary>
public class Category : BaseEntity
{
    public string Name { get; private set; } = string.Empty;
    public string? ParentId { get; private set; }
    public string? Description { get; private set; }

    private Category() { } // ORM / serialization

    /// <summary>
    /// Factory method — creates a valid Category.
    /// </summary>
    public static Category Create(string name, string? parentId = null, string? description = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Category name cannot be empty.");

        return new Category
        {
            Name = name.Trim(),
            ParentId = parentId?.Trim(),
            Description = description?.Trim()
        };
    }

    /// <summary>Renames this category.</summary>
    public void Rename(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
            throw new DomainException("Category name cannot be empty.");

        Name = newName.Trim();
        Touch();
    }

    /// <summary>Updates the optional description.</summary>
    public void UpdateDescription(string? description)
    {
        Description = description?.Trim();
        Touch();
    }

    /// <summary>Moves this category under a different parent (or root).</summary>
    public void MoveToParent(string? newParentId)
    {
        if (newParentId == Id)
            throw new DomainException("A category cannot be its own parent.");

        ParentId = newParentId?.Trim();
        Touch();
    }
}
