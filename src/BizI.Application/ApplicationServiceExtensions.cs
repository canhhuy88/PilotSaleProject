using BizI.Application.Common;
using BizI.Application.Interfaces;
using BizI.Application.Mappings;
using BizI.Application.Seed;
using BizI.Application.Seed.Seeders;
using BizI.Application.Services;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace BizI.Application;

/// <summary>
/// Extension method registering all Application-layer services into the DI container.
/// Called from Program.cs — Infrastructure must be registered separately via AddInfrastructure().
/// </summary>
public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // ── MediatR (CQRS handlers + validation pipeline) ─────────────────────
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(ApplicationServiceExtensions).Assembly);
            cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });

        // ── FluentValidation ──────────────────────────────────────────────────
        services.AddValidatorsFromAssembly(typeof(ApplicationServiceExtensions).Assembly);

        // ── AutoMapper (Entity → DTO) ─────────────────────────────────────────
        // AutoMapper 16.x: use cfg lambda to avoid overload resolution ambiguity
        services.AddAutoMapper(cfg => cfg.AddProfile<AutoMapperProfile>());

        // ── Application Services ─────────────────────────────────────────────
        // IInventoryService implementation lives here in Application
        // Repository dependencies are resolved from Infrastructure
        services.AddScoped<IInventoryService, InventoryService>();

        // ── Seed (Application/Seed) ──────────────────────────────────────────
        services.AddScoped<RoleSeeder>();
        services.AddScoped<UserSeeder>();
        services.AddScoped<CategorySeeder>();
        services.AddScoped<ProductSeeder>();
        services.AddScoped<SeedService>();

        return services;
    }
}
