using LiteDB;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BizI.Infrastructure.Data.Migrations;

public class MigrationRunner
{
    private readonly ILiteDatabase _db;
    private readonly IEnumerable<IMigration> _migrations;
    private readonly ILogger<MigrationRunner> _logger;

    public MigrationRunner(ILiteDatabase db, IEnumerable<IMigration> migrations, ILogger<MigrationRunner> logger)
    {
        _db = db;
        _migrations = migrations;
        _logger = logger;
    }

    public void Run()
    {
        var collection = _db.GetCollection<MigrationRecord>("_migrations");
        var appliedMigrations = collection.FindAll().Select(x => x.Version).ToHashSet();

        var pendingMigrations = _migrations
            .OrderBy(x => x.Version)
            .Where(x => !appliedMigrations.Contains(x.Version))
            .ToList();

        if (!pendingMigrations.Any())
        {
            _logger.LogInformation("No pending migrations found.");
            return;
        }

        foreach (var migration in pendingMigrations)
        {
            _logger.LogInformation("Applying migration version {Version}...", migration.Version);
            migration.Up(_db);
            collection.Insert(new MigrationRecord
            {
                Version = migration.Version,
                AppliedAt = DateTime.UtcNow
            });
            _logger.LogInformation("Applied migration version {Version}.", migration.Version);
        }
    }
}
