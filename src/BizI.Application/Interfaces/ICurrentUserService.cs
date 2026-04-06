using System.Security.Claims;

namespace BizI.Application.Interfaces;

public interface ICurrentUserService
{
    Guid? UserId { get; }
    string? Username { get; }
    Guid? RoleId { get; }
    bool IsAuthenticated { get; }
    ClaimsPrincipal? User { get; }
}