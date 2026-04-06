using BizI.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace BizI.Infrastructure.Data.Migrations;

/// <summary>
/// Replaces the old LiteDB MigrationRunner.
/// Applies any pending EF Core migrations at startup (or on --migrate flag).
/// </summary>
public class MigrationRunner
{
    private readonly AppDbContext _context;
    private readonly ILogger<MigrationRunner> _logger;

    public MigrationRunner(AppDbContext context, ILogger<MigrationRunner> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Applies all pending EF Core migrations and creates the database if it does not exist.
    /// </summary>
    public async Task RunAsync()
    {
        var pending = (await _context.Database.GetPendingMigrationsAsync()).ToList();

        if (!pending.Any())
        {
            _logger.LogInformation("No pending EF Core migrations.");
            return;
        }

        _logger.LogInformation("Applying {Count} pending migration(s): {Migrations}",
            pending.Count, string.Join(", ", pending));

        await _context.Database.MigrateAsync();

        _logger.LogInformation("EF Core migrations applied successfully.");
    }

    /// <summary>Synchronous wrapper for use in non-async contexts.</summary>
    public void Run() => RunAsync().GetAwaiter().GetResult();
}
