using BizI.Application.Common;
using BizI.Domain.Enums;

namespace BizI.Application.Interfaces;

/// <summary>
/// Application-level authentication service contract.
/// Implemented in Infrastructure (has JWT/BCrypt dependencies).
/// </summary>
public interface IAuthService
{
    /// <summary>Login with username/password; returns JWT token on success.</summary>
    Task<CommandResult> LoginAsync(string username, string password);

    /// <summary>Register a new user account.</summary>
    Task<CommandResult> RegisterAsync(string username, string password, UserRole role);

    /// <summary>Refresh access token using refresh token.</summary>
    Task<CommandResult> RefreshTokenAsync(string refreshToken);

    /// <summary>Revoke a refresh token.</summary>
    Task<CommandResult> RevokeTokenAsync(string refreshToken);
}
