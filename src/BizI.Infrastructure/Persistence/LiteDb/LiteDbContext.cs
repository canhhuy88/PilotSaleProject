using LiteDB;
using BizI.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace BizI.Infrastructure.Persistence.LiteDb;

/// <summary>
/// LiteDB implementation of <see cref="ILiteDbContext"/>.
/// All database-specific configuration (indices, mappings) lives here.
/// To switch databases, create a new context class — no other layers change.
/// </summary>
public sealed class LiteDbContext : ILiteDbContext
{
    private readonly ILogger<LiteDbContext> _logger;
    public LiteDatabase Database { get; }

    public LiteDbContext(string connectionString, ILogger<LiteDbContext> logger)
    {
        _logger = logger;
        Database = new LiteDatabase(connectionString);
        _logger.LogInformation("LiteDB context initialized. Connection: {ConnectionString}", connectionString);
        ConfigureIndices();
    }

    /// <inheritdoc />
    public ILiteCollection<T> GetCollection<T>(string name) =>
        Database.GetCollection<T>(name);

    private void ConfigureIndices()
    {
        _logger.LogDebug("Configuring LiteDB indices...");

        var products = Database.GetCollection<Product>(CollectionNames.Products);
        products.EnsureIndex(x => x.SKU, unique: true);
        products.EnsureIndex(x => x.Barcode);

        var customers = Database.GetCollection<Customer>(CollectionNames.Customers);
        customers.EnsureIndex(x => x.Phone);

        var orders = Database.GetCollection<Order>(CollectionNames.Orders);
        orders.EnsureIndex(x => x.Code);

        var stockItems = Database.GetCollection<StockItem>(CollectionNames.StockItems);
        stockItems.EnsureIndex(
            "ProductId_WarehouseId",
            $"$.{nameof(StockItem.ProductId)} + '_' + $.{nameof(StockItem.WarehouseId)}",
            unique: false);

        var categories = Database.GetCollection<Category>(CollectionNames.Categories);
        categories.EnsureIndex(x => x.Name);

        var users = Database.GetCollection<User>(CollectionNames.Users);
        users.EnsureIndex(x => x.Username, unique: true);

        var roles = Database.GetCollection<Role>(CollectionNames.Roles);
        roles.EnsureIndex(x => x.Name, unique: true);

        var inventory = Database.GetCollection<Inventory>(CollectionNames.Inventory);
        inventory.EnsureIndex(
            "ProductId_WarehouseId",
            $"$.{nameof(Inventory.ProductId)} + '_' + $.{nameof(Inventory.WarehouseId)}",
            unique: false);

        _logger.LogDebug("LiteDB indices configured successfully.");
    }

    public void Dispose()
    {
        Database.Dispose();
        _logger.LogInformation("LiteDB context disposed.");
    }
}
