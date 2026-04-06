using LiteDB;

namespace BizI.Infrastructure.Data.Migrations;

public interface IMigration
{
    int Version { get; }
    void Up(ILiteDatabase db);
}
