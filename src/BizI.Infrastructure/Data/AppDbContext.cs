using LiteDB;
using BizI.Domain.Entities;
using System;

namespace BizI.Infrastructure.Data;

public class AppDbContext : IDisposable
{
    public LiteDatabase Database { get; }

    public AppDbContext(string connectionString)
    {
        Database = new LiteDatabase(connectionString);
        ConfigureIndices();
    }

    private void ConfigureIndices()
    {
        var products = Database.GetCollection<Product>("Products");
        products.EnsureIndex(x => x.SKU, true);
        products.EnsureIndex(x => x.Barcode);

        var customers = Database.GetCollection<Customer>("Customers");
        customers.EnsureIndex(x => x.Phone);

        var orders = Database.GetCollection<Order>("Orders");
        orders.EnsureIndex(x => x.Code);

        var stockItems = Database.GetCollection<StockItem>("StockItems");
        // LiteDB index on combination of fields
        stockItems.EnsureIndex("ProductId_WarehouseId", $"$.{nameof(StockItem.ProductId)} + '_' + $.{nameof(StockItem.WarehouseId)}", false);

        var categories = Database.GetCollection<Category>("Categories");
        categories.EnsureIndex(x => x.Name);

        var users = Database.GetCollection<User>("Users");
        users.EnsureIndex(x => x.Username, true);

        var roles = Database.GetCollection<Role>("Roles");
        roles.EnsureIndex(x => x.Name, true);

        var inventory = Database.GetCollection<BizI.Domain.Entities.Inventory>("Inventory");
        inventory.EnsureIndex("ProductId_WarehouseId", $"$.{nameof(BizI.Domain.Entities.Inventory.ProductId)} + '_' + $.{nameof(BizI.Domain.Entities.Inventory.WarehouseId)}", false);
    }

    public void Dispose()
    {
        Database.Dispose();
    }
}