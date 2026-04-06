namespace BizI.Infrastructure.Persistence.LiteDb;

/// <summary>
/// Centralized LiteDB collection name constants.
/// Avoids magic strings scattered across repositories.
/// </summary>
public static class CollectionNames
{
    public const string Products = "Products";
    public const string Categories = "Categories";
    public const string Customers = "Customers";
    public const string CustomerGroups = "CustomerGroups";
    public const string Orders = "Orders";
    public const string Inventory = "Inventory";
    public const string InventoryTransactions = "InventoryTransactions";
    public const string Suppliers = "Suppliers";
    public const string Warehouses = "Warehouses";
    public const string Users = "Users";
    public const string Roles = "Roles";
    public const string StockItems = "StockItems";
    public const string StockTransactions = "StockTransactions";
    public const string ImportOrders = "ImportOrders";
    public const string PaymentAndReturns = "PaymentAndReturns";
    public const string StockOperations = "StockOperations";
    public const string AuditLogs = "AuditLogs";
    public const string Migrations = "Migrations";
}
