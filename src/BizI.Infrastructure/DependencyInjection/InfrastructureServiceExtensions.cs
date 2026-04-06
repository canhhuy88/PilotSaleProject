using BizI.Domain.Interfaces;
using BizI.Infrastructure.Auth;
using BizI.Infrastructure.Data.Migrations;
using BizI.Infrastructure.Data.Seeding;
using BizI.Infrastructure.Persistence.LiteDb;
using BizI.Infrastructure.Persistence.Repositories;
using BizI.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace BizI.Infrastructure.DependencyInjection;

/// <summary>
/// Extension method that registers all Infrastructure-layer services into the DI container.
/// <para>
/// To switch databases in the future:
/// <list type="number">
///   <item>Create a new DbContext (e.g. <c>MySqlDbContext</c>) implementing <see cref="ILiteDbContext"/> or a new abstraction.</item>
///   <item>Create new repository implementations for that database.</item>
///   <item>Change the registrations inside <see cref="AddInfrastructure"/> only — no other layers change.</item>
/// </list>
/// </para>
/// </summary>
public static class InfrastructureServiceExtensions
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        RegisterDatabase(services, configuration);
        RegisterRepositories(services);
        RegisterApplicationServices(services);
        RegisterMigrationsAndSeeding(services);

        return services;
    }

    // ── Database ─────────────────────────────────────────────────────────────

    private static void RegisterDatabase(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection") ?? "biz_i.db";

        // Register as Singleton because LiteDB uses a single file-level lock.
        services.AddSingleton<ILiteDbContext>(sp =>
        {
            var logger = sp.GetRequiredService<ILogger<LiteDbContext>>();
            return new LiteDbContext(connectionString, logger);
        });
    }

    // ── Repositories ─────────────────────────────────────────────────────────

    private static void RegisterRepositories(IServiceCollection services)
    {
        // Generic fallback — handles any entity not covered by a specialized repo.
        services.AddScoped(typeof(IRepository<>), typeof(LiteDbRepository<>));

        // Domain-specific repository interfaces → LiteDB implementations.
        // Swap these lines with MySqlProductRepository, etc. to switch database.
        services.AddScoped<IProductRepository, LiteDbProductRepository>();
        services.AddScoped<IInventoryRepository, LiteDbInventoryRepository>();
        services.AddScoped<IOrderRepository, LiteDbOrderRepository>();
    }

    // ── Application Services ─────────────────────────────────────────────────
    // NOTE: IInventoryService is registered in AddApplication() (Application layer).
    // Only infrastructure-specific services (Auth, TenantProvider) are registered here.

    private static void RegisterApplicationServices(IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddScoped<ITenantProvider, TenantProvider>();
        services.AddScoped<IAuthService, AuthService>();
    }

    // ── Migrations & Seeding ─────────────────────────────────────────────────

    private static void RegisterMigrationsAndSeeding(IServiceCollection services)
    {
        services.AddTransient<IMigration, InitialMigration>();
        services.AddScoped<MigrationRunner>();
        services.AddScoped<IDataSeeder, DataSeeder>();
    }
}
