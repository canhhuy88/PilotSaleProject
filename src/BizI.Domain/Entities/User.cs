using BizI.Domain.Exceptions;

namespace BizI.Domain.Entities;

/// <summary>
/// Represents a system user (staff member / administrator).
/// All sensitive mutations go through explicit domain methods.
/// </summary>
public class User : BaseEntity
{
    public string Username { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public string FullName { get; private set; } = string.Empty;
    public Guid RoleId { get; private set; }
    public Guid BranchId { get; private set; }
    public bool IsActive { get; private set; } = true;

    private User() { } // ORM / serialization

    // ──────────────────────────────────────────────
    //  Factory
    // ──────────────────────────────────────────────

    /// <summary>
    /// Creates a valid system user.
    /// Note: passwordHash must be pre-hashed by the application layer — never store plaintext.
    /// </summary>
    public static User Create(
        string username,
        string passwordHash,
        string fullName,
        Guid roleId,
        Guid branchId = default)
    {
        if (string.IsNullOrWhiteSpace(username))
            throw new DomainException("Username cannot be empty.");

        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new DomainException("Password hash cannot be empty.");

        if (string.IsNullOrWhiteSpace(fullName))
            throw new DomainException("Full name cannot be empty.");

        if (roleId == Guid.Empty) throw new DomainException("RoleId cannot be empty.");

        return new User
        {
            Username = username.Trim().ToLowerInvariant(),
            PasswordHash = passwordHash,
            FullName = fullName.Trim(),
            RoleId = roleId,
            BranchId = branchId
        };
    }

    // ──────────────────────────────────────────────
    //  Domain Methods
    // ──────────────────────────────────────────────

    /// <summary>Updates the user's display name and branch assignment.</summary>
    public void UpdateProfile(string fullName, Guid branchId)
    {
        if (string.IsNullOrWhiteSpace(fullName))
            throw new DomainException("Full name cannot be empty.");

        FullName = fullName.Trim();
        BranchId = branchId;
        Touch();
    }

    /// <summary>Replaces the stored password hash (application layer must hash before calling).</summary>
    public void ChangePassword(string newPasswordHash)
    {
        if (string.IsNullOrWhiteSpace(newPasswordHash))
            throw new DomainException("New password hash cannot be empty.");

        PasswordHash = newPasswordHash;
        Touch();
    }

    /// <summary>Re-assigns the user to a different role.</summary>
    public void AssignRole(Guid roleId)
    {
        if (roleId == Guid.Empty) throw new DomainException("RoleId cannot be empty.");

        RoleId = roleId;
        Touch();
    }

    /// <summary>Activates the user account.</summary>
    public void Activate()
    {
        IsActive = true;
        Touch();
    }

    /// <summary>Deactivates the user account (soft-disable).</summary>
    public void Deactivate()
    {
        IsActive = false;
        Touch();
    }
}
