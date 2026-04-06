using BizI.Domain.Exceptions;

namespace BizI.Domain.Entities;

/// <summary>
/// Represents a security role with a named set of permissions.
/// </summary>
public class Role : BaseEntity
{
    public string Name { get; private set; } = string.Empty;

    private readonly List<string> _permissions = new();

    /// <summary>Read-only view of this role's permissions.</summary>
    public IReadOnlyCollection<string> Permissions => _permissions.AsReadOnly();

    private Role() { } // ORM / serialization

    /// <summary>Factory — creates a valid Role.</summary>
    public static Role Create(string name, IEnumerable<string>? permissions = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Role name cannot be empty.");

        var role = new Role { Name = name.Trim() };

        if (permissions is not null)
            foreach (var p in permissions.Where(x => !string.IsNullOrWhiteSpace(x)))
                role._permissions.Add(p.Trim());

        return role;
    }

    /// <summary>Renames the role.</summary>
    public void Rename(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
            throw new DomainException("Role name cannot be empty.");

        Name = newName.Trim();
        Touch();
    }

    /// <summary>Grants a permission to this role.</summary>
    public void GrantPermission(string permission)
    {
        if (string.IsNullOrWhiteSpace(permission))
            throw new DomainException("Permission cannot be empty.");

        var p = permission.Trim();
        if (!_permissions.Contains(p, StringComparer.OrdinalIgnoreCase))
        {
            _permissions.Add(p);
            Touch();
        }
    }

    /// <summary>Revokes a permission from this role.</summary>
    public void RevokePermission(string permission)
    {
        var p = permission.Trim();
        var removed = _permissions.RemoveAll(x =>
            string.Equals(x, p, StringComparison.OrdinalIgnoreCase)) > 0;

        if (removed) Touch();
    }

    /// <summary>Returns true if this role has the specified permission.</summary>
    public bool HasPermission(string permission) =>
        _permissions.Any(p => string.Equals(p, permission, StringComparison.OrdinalIgnoreCase));

    /// <summary>Replaces all permissions with the supplied set.</summary>
    public void SetPermissions(IEnumerable<string> permissions)
    {
        _permissions.Clear();
        foreach (var p in permissions.Where(x => !string.IsNullOrWhiteSpace(x)))
            _permissions.Add(p.Trim());
        Touch();
    }
}
