using BizI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BizI.Infrastructure.Persistence.Configurations;

// ── AuditLog ──────────────────────────────────────────────────────────────────

public class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
{
    public void Configure(EntityTypeBuilder<AuditLog> entity)
    {
        entity.HasKey(x => x.Id);
        entity.Property(x => x.Id).HasMaxLength(36).IsRequired();

        entity.Property(x => x.Action).IsRequired().HasMaxLength(100);
        entity.Property(x => x.EntityName).IsRequired().HasMaxLength(100);
        entity.Property(x => x.EntityId).IsRequired().HasMaxLength(36);
        entity.Property(x => x.CreatedBy).HasMaxLength(100);
        entity.Property(x => x.OldData).HasColumnType("text");
        entity.Property(x => x.NewData).HasColumnType("text");
        entity.Property(x => x.CreatedAt);
        entity.Property(x => x.UpdatedAt);
        entity.Property(x => x.IsDeleted);
    }
}

// ── Category ──────────────────────────────────────────────────────────────────

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> entity)
    {
        entity.HasKey(x => x.Id);
        entity.Property(x => x.Id).HasMaxLength(36).IsRequired();
        entity.Property(x => x.Name).IsRequired().HasMaxLength(256);
        entity.HasIndex(x => x.Name);
        entity.Property(x => x.CreatedAt);
        entity.Property(x => x.UpdatedAt);
        entity.Property(x => x.IsDeleted);
    }
}

// ── CustomerGroup ─────────────────────────────────────────────────────────────

public class CustomerGroupConfiguration : IEntityTypeConfiguration<CustomerGroup>
{
    public void Configure(EntityTypeBuilder<CustomerGroup> entity)
    {
        entity.HasKey(x => x.Id);
        entity.Property(x => x.Id).HasMaxLength(36).IsRequired();
        entity.Property(x => x.Name).IsRequired().HasMaxLength(256);
        entity.Property(x => x.CreatedAt);
        entity.Property(x => x.UpdatedAt);
        entity.Property(x => x.IsDeleted);
    }
}

// ── Warehouse ─────────────────────────────────────────────────────────────────

public class WarehouseConfiguration : IEntityTypeConfiguration<Warehouse>
{
    public void Configure(EntityTypeBuilder<Warehouse> entity)
    {
        entity.HasKey(x => x.Id);
        entity.Property(x => x.Id).HasMaxLength(36).IsRequired();
        entity.Property(x => x.Name).IsRequired().HasMaxLength(256);
        entity.Property(x => x.CreatedAt);
        entity.Property(x => x.UpdatedAt);
        entity.Property(x => x.IsDeleted);
    }
}

// ── Supplier ──────────────────────────────────────────────────────────────────

public class SupplierConfiguration : IEntityTypeConfiguration<Supplier>
{
    public void Configure(EntityTypeBuilder<Supplier> entity)
    {
        entity.HasKey(x => x.Id);
        entity.Property(x => x.Id).HasMaxLength(36).IsRequired();
        entity.Property(x => x.Name).IsRequired().HasMaxLength(256);
        entity.Property(x => x.CreatedAt);
        entity.Property(x => x.UpdatedAt);
        entity.Property(x => x.IsDeleted);

        // PhoneNumber value object — stored as single string column
        entity.OwnsOne(x => x.Phone, phone =>
        {
            phone.Property(p => p.Value).HasColumnName("Phone").HasMaxLength(30);
        });

        // Address value object — stored as own columns
        entity.OwnsOne(x => x.ContactAddress, addr =>
        {
            addr.Property(a => a.Street).HasColumnName("Address_Street").HasMaxLength(500);
            addr.Property(a => a.District).HasColumnName("Address_District").HasMaxLength(200);
            addr.Property(a => a.City).HasColumnName("Address_City").HasMaxLength(200);
            addr.Property(a => a.Province).HasColumnName("Address_Province").HasMaxLength(200);
            addr.Property(a => a.Country).HasColumnName("Address_Country").HasMaxLength(100);
        });
    }
}

// ── ImportOrder ───────────────────────────────────────────────────────────────

public class ImportOrderConfiguration : IEntityTypeConfiguration<ImportOrder>
{
    public void Configure(EntityTypeBuilder<ImportOrder> entity)
    {
        entity.HasKey(x => x.Id);
        entity.Property(x => x.Id).HasMaxLength(36).IsRequired();

        entity.Property(x => x.SupplierId).IsRequired().HasMaxLength(36);
        entity.HasIndex(x => x.SupplierId);

        entity.Property(x => x.Status).IsRequired().HasMaxLength(50);

        entity.Property(x => x.CreatedAt);
        entity.Property(x => x.UpdatedAt);
        entity.Property(x => x.IsDeleted);

        // Money value object — must be mapped as owned type
        entity.OwnsOne(x => x.TotalAmount, money =>
        {
            money.Property(m => m.Amount).HasColumnName("TotalAmount_Amount").HasColumnType("decimal(18,2)");
            money.Property(m => m.Currency).HasColumnName("TotalAmount_Currency").HasMaxLength(10);
        });

        // Items collection stored in a separate table
        entity.HasMany(x => x.Items)
              .WithOne()
              .HasForeignKey("ImportOrderId")
              .OnDelete(DeleteBehavior.Cascade);

        entity.Navigation(x => x.Items).HasField("_items");
    }
}

// ── ImportOrderItem ───────────────────────────────────────────────────────────

public class ImportOrderItemConfiguration : IEntityTypeConfiguration<ImportOrderItem>
{
    public void Configure(EntityTypeBuilder<ImportOrderItem> entity)
    {
        entity.ToTable("ImportOrderItems");

        entity.Property<int>("Id").ValueGeneratedOnAdd();
        entity.HasKey("Id");

        entity.Property(x => x.ProductId).IsRequired().HasMaxLength(36);
        entity.Property(x => x.Quantity).IsRequired();

        // Money value object — must be mapped as owned type
        entity.OwnsOne(x => x.CostPrice, money =>
        {
            money.Property(m => m.Amount).HasColumnName("CostPrice_Amount").HasColumnType("decimal(18,2)");
            money.Property(m => m.Currency).HasColumnName("CostPrice_Currency").HasMaxLength(10);
        });
    }
}

// ── Role ──────────────────────────────────────────────────────────────────────

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> entity)
    {
        entity.HasKey(x => x.Id);
        entity.Property(x => x.Id).HasMaxLength(36).IsRequired();
        entity.Property(x => x.Name).IsRequired().HasMaxLength(100);
        entity.HasIndex(x => x.Name).IsUnique();
        entity.Property(x => x.CreatedAt);
        entity.Property(x => x.UpdatedAt);
        entity.Property(x => x.IsDeleted);
    }
}

// ── StockItem ─────────────────────────────────────────────────────────────────

public class StockItemConfiguration : IEntityTypeConfiguration<StockItem>
{
    public void Configure(EntityTypeBuilder<StockItem> entity)
    {
        entity.HasKey(x => x.Id);
        entity.Property(x => x.Id).HasMaxLength(36).IsRequired();
        entity.Property(x => x.ProductId).IsRequired().HasMaxLength(36);
        entity.Property(x => x.WarehouseId).IsRequired().HasMaxLength(36);
        entity.HasIndex(x => new { x.ProductId, x.WarehouseId });
        entity.Property(x => x.CreatedAt);
        entity.Property(x => x.UpdatedAt);
        entity.Property(x => x.IsDeleted);
    }
}

// ── StockTransaction ──────────────────────────────────────────────────────────

public class StockTransactionConfiguration : IEntityTypeConfiguration<StockTransaction>
{
    public void Configure(EntityTypeBuilder<StockTransaction> entity)
    {
        entity.HasKey(x => x.Id);
        entity.Property(x => x.Id).HasMaxLength(36).IsRequired();
        entity.Property(x => x.CreatedAt);
        entity.Property(x => x.UpdatedAt);
        entity.Property(x => x.IsDeleted);
    }
}

// ── InventoryTransaction ──────────────────────────────────────────────────────

public class InventoryTransactionConfiguration : IEntityTypeConfiguration<InventoryTransaction>
{
    public void Configure(EntityTypeBuilder<InventoryTransaction> entity)
    {
        entity.HasKey(x => x.Id);
        entity.Property(x => x.Id).HasMaxLength(36).IsRequired();
        entity.Property(x => x.CreatedAt);
        entity.Property(x => x.UpdatedAt);
        entity.Property(x => x.IsDeleted);
    }
}

// ── ProductVariant ────────────────────────────────────────────────────────────

public class ProductVariantConfiguration : IEntityTypeConfiguration<ProductVariant>
{
    public void Configure(EntityTypeBuilder<ProductVariant> entity)
    {
        entity.HasKey(x => x.Id);
        entity.Property(x => x.Id).HasMaxLength(36).IsRequired();

        entity.Property(x => x.ProductId).IsRequired().HasMaxLength(36);
        entity.HasIndex(x => x.ProductId);

        entity.Property(x => x.SKU).IsRequired().HasMaxLength(100);
        entity.HasIndex(x => x.SKU).IsUnique();

        entity.Property(x => x.Barcode).HasMaxLength(100);

        entity.Property(x => x.CreatedAt);
        entity.Property(x => x.UpdatedAt);
        entity.Property(x => x.IsDeleted);

        // Money value object — must be mapped as owned type
        entity.OwnsOne(x => x.Price, money =>
        {
            money.Property(m => m.Amount).HasColumnName("Price_Amount").HasColumnType("decimal(18,2)");
            money.Property(m => m.Currency).HasColumnName("Price_Currency").HasMaxLength(10);
        });

        // IReadOnlyDictionary<string,string> cannot be mapped by EF Core directly — store as JSON string
        entity.Property(x => x.Attributes)
              .HasConversion(
                  v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions?)null),
                  v => (IReadOnlyDictionary<string, string>)(System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(v, (System.Text.Json.JsonSerializerOptions?)null) ?? new Dictionary<string, string>()))
              .HasColumnType("text")
              .Metadata.SetValueComparer(new Microsoft.EntityFrameworkCore.ChangeTracking.ValueComparer<IReadOnlyDictionary<string, string>>(
                  (c1, c2) => System.Text.Json.JsonSerializer.Serialize(c1, (System.Text.Json.JsonSerializerOptions?)null) ==
                               System.Text.Json.JsonSerializer.Serialize(c2, (System.Text.Json.JsonSerializerOptions?)null),
                  c => c == null ? 0 : System.Text.Json.JsonSerializer.Serialize(c, (System.Text.Json.JsonSerializerOptions?)null).GetHashCode(),
                  c => (IReadOnlyDictionary<string, string>)(System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(
                      System.Text.Json.JsonSerializer.Serialize(c, (System.Text.Json.JsonSerializerOptions?)null),
                      (System.Text.Json.JsonSerializerOptions?)null) ?? new Dictionary<string, string>())));
    }
}
