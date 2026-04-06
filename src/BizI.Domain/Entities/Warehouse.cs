using BizI.Domain.Exceptions;

namespace BizI.Domain.Entities;

/// <summary>
/// Represents a physical warehouse location belonging to a branch.
/// </summary>
public class Warehouse : BaseEntity
{
    public string Name { get; private set; } = string.Empty;
    public Guid BranchId { get; private set; }

    private Warehouse() { } // ORM / serialization

    /// <summary>Factory — creates a valid Warehouse.</summary>
    public static Warehouse Create(string name, Guid branchId)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Warehouse name cannot be empty.");

        if (branchId == Guid.Empty)
            throw new DomainException("BranchId cannot be empty.");

        return new Warehouse
        {
            Name = name.Trim(),
            BranchId = branchId
        };
    }

    /// <summary>Renames the warehouse.</summary>
    public void Rename(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
            throw new DomainException("Warehouse name cannot be empty.");

        Name = newName.Trim();
        Touch();
    }

    /// <summary>Reassigns this warehouse to a different branch.</summary>
    public void Reassign(Guid newBranchId)
    {
        if (newBranchId == Guid.Empty)
            throw new DomainException("BranchId cannot be empty.");

        BranchId = newBranchId;
        Touch();
    }
}
