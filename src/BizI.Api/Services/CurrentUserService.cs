using System.Security.Claims;
using BizI.Application.Interfaces;
using Microsoft.AspNetCore.Http;

namespace BizI.Api.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public ClaimsPrincipal? User => _httpContextAccessor.HttpContext?.User;

    public Guid? UserId => Guid.TryParse(User?.FindFirstValue(ClaimTypes.Name), out var id) ? id : null;

    public string? Username => User?.FindFirstValue(ClaimTypes.NameIdentifier);

    public Guid? RoleId => Guid.TryParse(User?.FindFirstValue(ClaimTypes.Role), out var rid) ? rid : null;

    public bool IsAuthenticated => User?.Identity?.IsAuthenticated ?? false;
}