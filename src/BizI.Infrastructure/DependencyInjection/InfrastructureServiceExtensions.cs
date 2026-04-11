using BizI.Domain.Interfaces;
using BizI.Infrastructure.Auth;
using BizI.Infrastructure.Data;
using BizI.Infrastructure.Data.Seeding;
using BizI.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BizI.Infrastructure.DependencyInjection;

/// <summary>
/// Extension method that registers all Infrastructure-layer services into the DI container.
/// <para>
/// To switch databases in the future — ONLY change the provider below:
/// <list type="number">
///   <item>SQLite:      options.UseSqlite(connectionString)</item>
///   <item>SQL Server:  options.UseSqlServer(connectionString)</item>
///   <item>PostgreSQL:  options.UseNpgsql(connectionString)</item>
/// </list>
/// No other layers (Domain, Application, API) ever change.
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
        RegisterSeeding(services);

        return services;
    }

    // ── Database ──────────────────────────────────────────────────────────────

    private static void RegisterDatabase(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<AppDbContext>(options =>
            // 🔁 Future-proof: swap UseSqlite → UseSqlServer / UseNpgsql here only
            options.UseSqlServer(connectionString));
        /*
        // SQLite (current)
        options.UseSqlite(connectionString)

        // → SQL Server
        options.UseSqlServer(connectionString)

        // → PostgreSQL
        options.UseNpgsql(connectionString)

        */
    }

    // ── Repositories ──────────────────────────────────────────────────────────

    private static void RegisterRepositories(IServiceCollection services)
    {
        // Generic fallback — handles any entity not covered by a specialized repo.
        services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));

        // Domain-specific repository interfaces → EF Core implementations.
        // Swap these lines when switching providers — e.g. future PgProductRepository.
        //services.AddScoped<IProductRepository, EfProductRepository>();
        //services.AddScoped<IInventoryRepository, EfInventoryRepository>();
        //services.AddScoped<IOrderRepository, EfOrderRepository>();
    }

    // ── Application Services ──────────────────────────────────────────────────
    // NOTE: IInventoryService is registered in AddApplication() (Application layer).
    // Only infrastructure-specific services (Auth, TenantProvider) are registered here.

    private static void RegisterApplicationServices(IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        //services.AddScoped<ITenantProvider, TenantProvider>();
        services.AddScoped<IAuthService, AuthService>();
    }

    // ── Seeding ───────────────────────────────────────────────────────────────
    // NOTE: MigrationRunner is REMOVED. All schema changes go through EF CLI.
    //   dotnet ef migrations add <Name> --project src/BizI.Infrastructure --startup-project src/BizI.Api
    //   dotnet ef database update       --project src/BizI.Infrastructure --startup-project src/BizI.Api

    private static void RegisterSeeding(IServiceCollection services)
    {
        services.AddScoped<IDataSeeder, DataSeeder>();
    }
}
