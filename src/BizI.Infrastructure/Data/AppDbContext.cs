using BizI.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BizI.Infrastructure.Data;

/// <summary>
/// EF Core DbContext for the BizI application.
/// All entity configurations are applied via Fluent API from the assembly.
/// To switch databases: only change the provider in DI
///   - SQLite:      options.UseSqlite(...)
///   - SQL Server:  options.UseSqlServer(...)
///   - PostgreSQL:  options.UseNpgsql(...)
/// No other layer changes.
/// </summary>
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    // ── Core aggregates ───────────────────────────────────────────────────────
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<CustomerGroup> CustomerGroups => Set<CustomerGroup>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();

    // ── Inventory ─────────────────────────────────────────────────────────────
    public DbSet<Inventory> Inventories => Set<Inventory>();
    public DbSet<InventoryTransaction> InventoryTransactions => Set<InventoryTransaction>();

    // ── Warehouse / Supplier / Import ─────────────────────────────────────────
    public DbSet<Warehouse> Warehouses => Set<Warehouse>();
    public DbSet<Supplier> Suppliers => Set<Supplier>();
    public DbSet<ImportOrder> ImportOrders => Set<ImportOrder>();

    // ── Stock operations ──────────────────────────────────────────────────────
    public DbSet<StockItem> StockItems => Set<StockItem>();
    public DbSet<StockTransaction> StockTransactions => Set<StockTransaction>();
    public DbSet<StockIn> StockIns => Set<StockIn>();
    public DbSet<StockInItem> StockInItems => Set<StockInItem>();
    public DbSet<StockOut> StockOuts => Set<StockOut>();
    public DbSet<StockOutItem> StockOutItems => Set<StockOutItem>();
    public DbSet<StockTransfer> StockTransfers => Set<StockTransfer>();
    public DbSet<StockTransferItem> StockTransferItems => Set<StockTransferItem>();
    public DbSet<StockAudit> StockAudits => Set<StockAudit>();
    public DbSet<StockAuditItem> StockAuditItems => Set<StockAuditItem>();

    // ── Payment & Returns ─────────────────────────────────────────────────────
    public DbSet<Payment> Payments => Set<Payment>();
    public DbSet<Debt> Debts => Set<Debt>();
    public DbSet<ReturnOrder> ReturnOrders => Set<ReturnOrder>();
    public DbSet<ReturnItem> ReturnItems => Set<ReturnItem>();

    // ── Users / Roles ─────────────────────────────────────────────────────────
    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();

    // ── Products ──────────────────────────────────────────────────────────────
    public DbSet<ProductVariant> ProductVariants => Set<ProductVariant>();

    // ── Audit ─────────────────────────────────────────────────────────────────
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Discover and apply every IEntityTypeConfiguration<T> in this assembly
        builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
