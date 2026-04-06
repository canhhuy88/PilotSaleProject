//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Design;

//namespace BizI.Infrastructure.Data;

///// <summary>
///// Design-time factory required by EF Core CLI tools.
///// Allows running:
/////   dotnet ef migrations add &lt;Name&gt; --project src/BizI.Infrastructure --startup-project src/BizI.Api
/////   dotnet ef database update         --project src/BizI.Infrastructure --startup-project src/BizI.Api
/////
///// ⚠️ This factory is ONLY used at design-time (CLI). At runtime, AppDbContext
/////    is resolved from the DI container via InfrastructureServiceExtensions.
///// </summary>
//public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
//{
//    public AppDbContext CreateDbContext(string[] args)
//    {
//        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

//        // ⚙️ Design-time uses the same default connection string.
//        // Override via: appsettings.json → ConnectionStrings:DefaultConnection
//        optionsBuilder.UseSqlite("Data Source=app.db");

//        return new AppDbContext(optionsBuilder.Options);
//    }
//}
