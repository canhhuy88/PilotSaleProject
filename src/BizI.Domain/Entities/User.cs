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
    public string RoleId { get; private set; } = string.Empty;
    public string BranchId { get; private set; } = string.Empty;
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
        string roleId,
        string branchId = "")
    {
        if (string.IsNullOrWhiteSpace(username))
            throw new DomainException("Username cannot be empty.");

        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new DomainException("Password hash cannot be empty.");

        if (string.IsNullOrWhiteSpace(fullName))
            throw new DomainException("Full name cannot be empty.");

        if (string.IsNullOrWhiteSpace(roleId))
            throw new DomainException("RoleId cannot be empty.");

        return new User
        {
            Username = username.Trim().ToLowerInvariant(),
            PasswordHash = passwordHash,
            FullName = fullName.Trim(),
            RoleId = roleId.Trim(),
            BranchId = branchId.Trim()
        };
    }

    // ──────────────────────────────────────────────
    //  Domain Methods
    // ──────────────────────────────────────────────

    /// <summary>Updates the user's display name and branch assignment.</summary>
    public void UpdateProfile(string fullName, string branchId)
    {
        if (string.IsNullOrWhiteSpace(fullName))
            throw new DomainException("Full name cannot be empty.");

        FullName = fullName.Trim();
        BranchId = branchId.Trim();
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
    public void AssignRole(string roleId)
    {
        if (string.IsNullOrWhiteSpace(roleId))
            throw new DomainException("RoleId cannot be empty.");

        RoleId = roleId.Trim();
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
