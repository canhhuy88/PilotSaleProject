using Azure;
using BizI.Application.Interfaces;
using BizI.Domain.Entities;
using BizI.Domain.Enums;
using BizI.Infrastructure.Auth;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace BizI.Api.Endpoints;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/auth").WithTags("Auth");

        group.MapPost("login", async (LoginRequest request, IAuthService authService, HttpContext context) =>
        {
            var result = await authService.LoginAsync(request.Username, request.Password);

            var successResult = SetTokenCookieAndReturnResponse(result, context);
            if (successResult != null)
            {
                return successResult;
            }

            return Results.BadRequest(result.Message);
        });

        group.MapPost("register", async (RegisterRequest request, IAuthService authService) =>
        {
            var result = await authService.RegisterAsync(request.Username, request.Password, UserRole.Admin);
            return result.Success ? Results.Ok() : Results.BadRequest(result.Message);
        });

        group.MapPost("refresh", async (IAuthService authService, HttpContext context) =>
        {
            var refreshToken = context.Request.Cookies["refreshToken"];
            if (string.IsNullOrEmpty(refreshToken))
            {
                return Results.Unauthorized();
            }

            var result = await authService.RefreshTokenAsync(refreshToken);

            var successResult = SetTokenCookieAndReturnResponse(result, context);
            if (successResult != null)
            {
                return successResult;
            }

            return Results.Unauthorized();
        });

        group.MapPost("revoke", async (IAuthService authService, HttpContext context) =>
        {
            var refreshToken = context.Request.Cookies["refreshToken"];
            if (string.IsNullOrEmpty(refreshToken))
            {
                return Results.BadRequest("No refresh token found.");
            }

            var result = await authService.RevokeTokenAsync(refreshToken);
            if (result.Success)
            {
                context.Response.Cookies.Delete("refreshToken", new CookieOptions { Path = "/api/auth/refresh" });
                return Results.Ok();
            }

            return Results.BadRequest(result.Message);
        });
    }

    private static IResult? SetTokenCookieAndReturnResponse(BizI.Application.Common.CommandResult result, HttpContext context)
    {
        if (result.Success && result.Data is AuthDto authData)
        {
            var tokenResponse = new TokenResponse(
                AccessToken: authData.AccessToken,
                Username: authData.Username,
                Role: authData.Role.ToString()
            );

            // 3. Cấu hình Cookie Options
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,        // Quan trọng: JavaScript không thể truy cập
                Secure = true,          // Chỉ gửi qua HTTPS (nên bật khi chạy thật)
                SameSite = SameSiteMode.Strict, // Chống tấn công CSRF
                Expires = DateTime.UtcNow.AddDays(7), // Thời gian hết hạn của cookie
                Path = "/api/auth/refresh" // Chỉ gửi cookie này khi gọi đến đúng endpoint refresh
            };
            context.Response.Cookies.Append("refreshToken", authData.RefreshToken ?? string.Empty, cookieOptions);
            return Results.Ok(tokenResponse);
        }

        return null;
    }
}

public record LoginRequest(string Username, string Password);
public record RegisterRequest(string Username, string Password);
public record RefreshTokenRequest(string RefreshToken);
public record RevokeTokenRequest(string RefreshToken);
public record TokenResponse(string AccessToken, string Username, string Role);
