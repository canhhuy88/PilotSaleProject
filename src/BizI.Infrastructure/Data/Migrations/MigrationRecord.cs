using System;

namespace BizI.Infrastructure.Data.Migrations;

public class MigrationRecord
{
    public int Id { get; set; }
    public int Version { get; set; }
    public DateTime AppliedAt { get; set; }
}
