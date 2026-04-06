using System.Security.Claims;

namespace BizI.Application.Interfaces;

public interface ICurrentUserService
{
    string? UserId { get; }
    string? Username { get; }
    string? RoleId { get; }
    bool IsAuthenticated { get; }
    ClaimsPrincipal? User { get; }
}