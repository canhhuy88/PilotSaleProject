using LiteDB;

namespace BizI.Infrastructure.Persistence.LiteDb;

/// <summary>
/// Database context abstraction — decouples the Infrastructure layer from LiteDB specifics.
/// Future implementations (e.g. MySqlDbContext, PostgresDbContext) implement this interface
/// without any changes to Domain, Application, or API layers.
/// </summary>
public interface ILiteDbContext : IDisposable
{
    ILiteCollection<T> GetCollection<T>(string name);
}
