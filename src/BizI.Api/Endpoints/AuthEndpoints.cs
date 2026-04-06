using BizI.Application.Interfaces;
using BizI.Domain.Enums;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace BizI.Api.Endpoints;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/auth").WithTags("Auth");

        group.MapPost("login", async (LoginRequest request, IAuthService authService) =>
        {
            var result = await authService.LoginAsync(request.Username, request.Password);
            return result.Success ? Results.Ok(result.Data) : Results.Unauthorized();
        });

        group.MapPost("register", async (RegisterRequest request, IAuthService authService) =>
        {
            var result = await authService.RegisterAsync(request.Username, request.Password, UserRole.Admin);
            return result.Success ? Results.Ok() : Results.BadRequest(result.Message);
        });

        group.MapPost("refresh", async (RefreshTokenRequest request, IAuthService authService) =>
        {
            var result = await authService.RefreshTokenAsync(request.RefreshToken);
            return result.Success ? Results.Ok(result.Data) : Results.Unauthorized();
        });

        group.MapPost("revoke", async (RevokeTokenRequest request, IAuthService authService) =>
        {
            var result = await authService.RevokeTokenAsync(request.RefreshToken);
            return result.Success ? Results.Ok() : Results.BadRequest(result.Message);
        });
    }
}

public record LoginRequest(string Username, string Password);
public record RegisterRequest(string Username, string Password);
public record RefreshTokenRequest(string RefreshToken);
public record RevokeTokenRequest(string RefreshToken);
public record TokenResponse(string AccessToken, string RefreshToken, string Username, string Role);
