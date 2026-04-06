using Microsoft.AspNetCore.Http;
using Microsoft.Win32;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BizI.Api.Middleware;

public class TenantMiddleware
{
    private readonly RequestDelegate _next;

    public TenantMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Allow Swagger, Scalar and other public routes if needed, but the requirement says all queries must filter.
        if (context.Request.Path.StartsWithSegments("/swagger") ||
            context.Request.Path.StartsWithSegments("/scalar") ||
            context.Request.Path.StartsWithSegments("/openapi") ||
            context.Request.Path.StartsWithSegments("/favicon.ico") ||
            context.Request.Path.StartsWithSegments("/api/auth") ||
            context.Request.Path.StartsWithSegments("/api/register"))
            
        {
            await _next(context);
            return;
        }

        if (!context.Request.Headers.TryGetValue("X-Tenant-Id", out var tenantIdStr))
        {
            context.Response.StatusCode = 400; // Bad Request
            await context.Response.WriteAsync("X-Tenant-Id header is missing");
            return;
        }

        if (!Guid.TryParse(tenantIdStr, out var tenantId))
        {
            context.Response.StatusCode = 400;
            await context.Response.WriteAsync("Invalid X-Tenant-Id format");
            return;
        }

        context.Items["TenantId"] = tenantId;
        await _next(context);
    }
}

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred.");
            context.Response.StatusCode = 500;
            await context.Response.WriteAsJsonAsync(new { Message = ex.Message });
        }
    }
}
