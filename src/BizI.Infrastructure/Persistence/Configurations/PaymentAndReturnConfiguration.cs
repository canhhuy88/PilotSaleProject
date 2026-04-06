using BizI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BizI.Infrastructure.Persistence.Configurations;

// ── Payment ───────────────────────────────────────────────────────────────────

public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> entity)
    {
        entity.HasKey(x => x.Id);
        entity.Property(x => x.Id).IsRequired();

        entity.Property(x => x.OrderId).IsRequired().HasMaxLength(36);
        entity.HasIndex(x => x.OrderId);

        entity.Property(x => x.Method).IsRequired().HasMaxLength(100);
        entity.Property(x => x.CreatedAt);
        entity.Property(x => x.UpdatedAt);
        entity.Property(x => x.IsDeleted);

        entity.OwnsOne(x => x.Amount, money =>
        {
            money.Property(m => m.Amount).HasColumnName("Amount_Value").HasColumnType("decimal(18,2)");
            money.Property(m => m.Currency).HasColumnName("Amount_Currency").HasMaxLength(10);
        });
    }
}

// ── Debt ──────────────────────────────────────────────────────────────────────

public class DebtConfiguration : IEntityTypeConfiguration<Debt>
{
    public void Configure(EntityTypeBuilder<Debt> entity)
    {
        entity.HasKey(x => x.Id);
        entity.Property(x => x.Id).IsRequired();

        entity.Property(x => x.CustomerId).IsRequired().HasMaxLength(36);
        entity.Property(x => x.OrderId).IsRequired().HasMaxLength(36);
        entity.Property(x => x.Status).IsRequired().HasMaxLength(50);
        entity.Property(x => x.CreatedAt);
        entity.Property(x => x.UpdatedAt);
        entity.Property(x => x.IsDeleted);

        entity.OwnsOne(x => x.Amount, money =>
        {
            money.Property(m => m.Amount).HasColumnName("Amount_Value").HasColumnType("decimal(18,2)");
            money.Property(m => m.Currency).HasColumnName("Amount_Currency").HasMaxLength(10);
        });

        entity.OwnsOne(x => x.PaidAmount, money =>
        {
            money.Property(m => m.Amount).HasColumnName("PaidAmount_Value").HasColumnType("decimal(18,2)");
            money.Property(m => m.Currency).HasColumnName("PaidAmount_Currency").HasMaxLength(10);
        });

        // RemainingAmount is a computed property — not persisted
        entity.Ignore(x => x.RemainingAmount);
    }
}

// ── ReturnOrder ───────────────────────────────────────────────────────────────

public class ReturnOrderConfiguration : IEntityTypeConfiguration<ReturnOrder>
{
    public void Configure(EntityTypeBuilder<ReturnOrder> entity)
    {
        entity.HasKey(x => x.Id);
        entity.Property(x => x.Id).IsRequired();

        entity.Property(x => x.OrderId).IsRequired().HasMaxLength(36);
        entity.HasIndex(x => x.OrderId);

        entity.Property(x => x.CreatedAt);
        entity.Property(x => x.UpdatedAt);
        entity.Property(x => x.IsDeleted);

        entity.OwnsOne(x => x.TotalRefund, money =>
        {
            money.Property(m => m.Amount).HasColumnName("TotalRefund_Amount").HasColumnType("decimal(18,2)");
            money.Property(m => m.Currency).HasColumnName("TotalRefund_Currency").HasMaxLength(10);
        });

        entity.HasMany(x => x.Items)
              .WithOne()
              .HasForeignKey("ReturnOrderId")
              .OnDelete(DeleteBehavior.Cascade);

        entity.Navigation(x => x.Items).HasField("_items");
    }
}

public class ReturnItemConfiguration : IEntityTypeConfiguration<ReturnItem>
{
    public void Configure(EntityTypeBuilder<ReturnItem> entity)
    {
        entity.ToTable("ReturnItems");
        entity.Property<int>("Id").ValueGeneratedOnAdd();
        entity.HasKey("Id");

        entity.Property(x => x.ProductId).IsRequired().HasMaxLength(36);
        entity.Property(x => x.Quantity).IsRequired();

        // LineRefund is computed — not persisted
        entity.Ignore(x => x.LineRefund);

        entity.OwnsOne(x => x.RefundPrice, money =>
        {
            money.Property(m => m.Amount).HasColumnName("RefundPrice_Amount").HasColumnType("decimal(18,2)");
            money.Property(m => m.Currency).HasColumnName("RefundPrice_Currency").HasMaxLength(10);
        });
    }
}
