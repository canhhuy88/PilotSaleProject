using System.Threading.Tasks;

namespace BizI.Infrastructure.Data.Seeding;

public interface IDataSeeder
{
    Task SeedAsync();
}
