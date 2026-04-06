using LiteDB;
using BizI.Domain.Entities;

namespace BizI.Infrastructure.Data.Migrations;

public class InitialMigration : IMigration
{
    public int Version => 1;

    public void Up(ILiteDatabase db)
    {
        var users = db.GetCollection<User>("User");
        users.EnsureIndex(x => x.Username, true);

        var products = db.GetCollection<Product>("Product");

        var inventory = db.GetCollection<Inventory>("Inventory");
        inventory.EnsureIndex(x => x.ProductId);
        inventory.EnsureIndex(x => x.WarehouseId);

        var warehouses = db.GetCollection<Warehouse>("Warehouse");

        var orders = db.GetCollection<Order>("Order");
        orders.EnsureIndex(x => x.CreatedAt);
    }
}
