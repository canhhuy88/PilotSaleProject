using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using BizI.Domain.Interfaces;

namespace BizI.Infrastructure.Services;

public class TenantProvider : ITenantProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public TenantProvider(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid GetTenantId()
    {
        var context = _httpContextAccessor.HttpContext;
        if (context == null) return Guid.Empty;

        var tenantIdStr = context.Request.Headers["X-Tenant-Id"].FirstOrDefault();
        if (Guid.TryParse(tenantIdStr, out var tenantId))
        {
            return tenantId;
        }

        // For some operations (like background tasks) TenantId might be in Items if middleware set it
        if (context.Items.TryGetValue("TenantId", out var itemTenantId) && itemTenantId is Guid guid)
        {
            return guid;
        }

        return Guid.Empty;
    }
}
