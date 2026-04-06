using BizI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BizI.Infrastructure.Persistence.Configurations;

// ── StockIn ───────────────────────────────────────────────────────────────────

public class StockInConfiguration : IEntityTypeConfiguration<StockIn>
{
    public void Configure(EntityTypeBuilder<StockIn> entity)
    {
        entity.HasKey(x => x.Id);
        entity.Property(x => x.Id).HasMaxLength(36).IsRequired();

        entity.Property(x => x.Code).IsRequired().HasMaxLength(50);
        entity.Property(x => x.SupplierId).IsRequired().HasMaxLength(36);
        entity.Property(x => x.WarehouseId).IsRequired().HasMaxLength(36);
        entity.Property(x => x.CreatedAt);
        entity.Property(x => x.UpdatedAt);
        entity.Property(x => x.IsDeleted);

        entity.OwnsOne(x => x.TotalAmount, money =>
        {
            money.Property(m => m.Amount).HasColumnName("TotalAmount_Amount").HasColumnType("decimal(18,2)");
            money.Property(m => m.Currency).HasColumnName("TotalAmount_Currency").HasMaxLength(10);
        });

        entity.HasMany(x => x.Items)
              .WithOne()
              .HasForeignKey("StockInId")
              .OnDelete(DeleteBehavior.Cascade);

        entity.Navigation(x => x.Items).HasField("_items");
    }
}

public class StockInItemConfiguration : IEntityTypeConfiguration<StockInItem>
{
    public void Configure(EntityTypeBuilder<StockInItem> entity)
    {
        entity.ToTable("StockInItems");
        entity.Property<int>("Id").ValueGeneratedOnAdd();
        entity.HasKey("Id");

        entity.Property(x => x.ProductId).IsRequired().HasMaxLength(36);
        entity.Property(x => x.Quantity).IsRequired();

        entity.OwnsOne(x => x.CostPrice, money =>
        {
            money.Property(m => m.Amount).HasColumnName("CostPrice_Amount").HasColumnType("decimal(18,2)");
            money.Property(m => m.Currency).HasColumnName("CostPrice_Currency").HasMaxLength(10);
        });
    }
}

// ── StockOut ──────────────────────────────────────────────────────────────────

public class StockOutConfiguration : IEntityTypeConfiguration<StockOut>
{
    public void Configure(EntityTypeBuilder<StockOut> entity)
    {
        entity.HasKey(x => x.Id);
        entity.Property(x => x.Id).HasMaxLength(36).IsRequired();

        entity.Property(x => x.Code).IsRequired().HasMaxLength(50);
        entity.Property(x => x.WarehouseId).IsRequired().HasMaxLength(36);
        entity.Property(x => x.Reason).IsRequired().HasMaxLength(500);
        entity.Property(x => x.CreatedAt);
        entity.Property(x => x.UpdatedAt);
        entity.Property(x => x.IsDeleted);

        entity.HasMany(x => x.Items)
              .WithOne()
              .HasForeignKey("StockOutId")
              .OnDelete(DeleteBehavior.Cascade);

        entity.Navigation(x => x.Items).HasField("_items");
    }
}

public class StockOutItemConfiguration : IEntityTypeConfiguration<StockOutItem>
{
    public void Configure(EntityTypeBuilder<StockOutItem> entity)
    {
        entity.ToTable("StockOutItems");
        entity.Property<int>("Id").ValueGeneratedOnAdd();
        entity.HasKey("Id");

        entity.Property(x => x.ProductId).IsRequired().HasMaxLength(36);
        entity.Property(x => x.Quantity).IsRequired();
    }
}

// ── StockTransfer ─────────────────────────────────────────────────────────────

public class StockTransferConfiguration : IEntityTypeConfiguration<StockTransfer>
{
    public void Configure(EntityTypeBuilder<StockTransfer> entity)
    {
        entity.HasKey(x => x.Id);
        entity.Property(x => x.Id).HasMaxLength(36).IsRequired();

        entity.Property(x => x.FromWarehouseId).IsRequired().HasMaxLength(36);
        entity.Property(x => x.ToWarehouseId).IsRequired().HasMaxLength(36);
        entity.Property(x => x.CreatedAt);
        entity.Property(x => x.UpdatedAt);
        entity.Property(x => x.IsDeleted);

        entity.HasMany(x => x.Items)
              .WithOne()
              .HasForeignKey("StockTransferId")
              .OnDelete(DeleteBehavior.Cascade);

        entity.Navigation(x => x.Items).HasField("_items");
    }
}

public class StockTransferItemConfiguration : IEntityTypeConfiguration<StockTransferItem>
{
    public void Configure(EntityTypeBuilder<StockTransferItem> entity)
    {
        entity.ToTable("StockTransferItems");
        entity.Property<int>("Id").ValueGeneratedOnAdd();
        entity.HasKey("Id");

        entity.Property(x => x.ProductId).IsRequired().HasMaxLength(36);
        entity.Property(x => x.Quantity).IsRequired();
    }
}

// ── StockAudit ────────────────────────────────────────────────────────────────

public class StockAuditConfiguration : IEntityTypeConfiguration<StockAudit>
{
    public void Configure(EntityTypeBuilder<StockAudit> entity)
    {
        entity.HasKey(x => x.Id);
        entity.Property(x => x.Id).HasMaxLength(36).IsRequired();

        entity.Property(x => x.WarehouseId).IsRequired().HasMaxLength(36);
        entity.Property(x => x.CreatedAt);
        entity.Property(x => x.UpdatedAt);
        entity.Property(x => x.IsDeleted);

        entity.HasMany(x => x.Items)
              .WithOne()
              .HasForeignKey("StockAuditId")
              .OnDelete(DeleteBehavior.Cascade);

        entity.Navigation(x => x.Items).HasField("_items");
    }
}

public class StockAuditItemConfiguration : IEntityTypeConfiguration<StockAuditItem>
{
    public void Configure(EntityTypeBuilder<StockAuditItem> entity)
    {
        entity.ToTable("StockAuditItems");
        entity.Property<int>("Id").ValueGeneratedOnAdd();
        entity.HasKey("Id");

        entity.Property(x => x.ProductId).IsRequired().HasMaxLength(36);
        entity.Property(x => x.SystemQty).IsRequired();
        entity.Property(x => x.ActualQty).IsRequired();
    }
}
