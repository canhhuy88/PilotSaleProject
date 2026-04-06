using BizI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BizI.Infrastructure.Persistence.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> entity)
    {
        entity.HasKey(x => x.Id);
        entity.Property(x => x.Id).IsRequired();

        entity.Property(x => x.Code).IsRequired().HasMaxLength(50);
        entity.HasIndex(x => x.Code);

        entity.Property(x => x.CustomerId).IsRequired().HasMaxLength(36);
        entity.HasIndex(x => x.CustomerId);

        entity.Property(x => x.CreatedBy).IsRequired().HasMaxLength(100);
        entity.Property(x => x.Status).HasConversion<string>().HasMaxLength(50);
        entity.Property(x => x.PaymentStatus).HasConversion<string>().HasMaxLength(50);
        entity.Property(x => x.CreatedAt);
        entity.Property(x => x.UpdatedAt);
        entity.Property(x => x.IsDeleted);

        entity.OwnsOne(x => x.TotalAmount, money =>
        {
            money.Property(m => m.Amount).HasColumnName("TotalAmount_Amount").HasColumnType("decimal(18,2)");
            money.Property(m => m.Currency).HasColumnName("TotalAmount_Currency").HasMaxLength(10);
        });

        entity.OwnsOne(x => x.Discount, money =>
        {
            money.Property(m => m.Amount).HasColumnName("Discount_Amount").HasColumnType("decimal(18,2)");
            money.Property(m => m.Currency).HasColumnName("Discount_Currency").HasMaxLength(10);
        });

        entity.OwnsOne(x => x.FinalAmount, money =>
        {
            money.Property(m => m.Amount).HasColumnName("FinalAmount_Amount").HasColumnType("decimal(18,2)");
            money.Property(m => m.Currency).HasColumnName("FinalAmount_Currency").HasMaxLength(10);
        });

        // OrderItems: owned collection stored in separate table, with FK back to Order
        entity.HasMany(x => x.Items)
              .WithOne()
              .HasForeignKey("OrderId")
              .OnDelete(DeleteBehavior.Cascade);

        // Access private backing field _items for navigation
        entity.Navigation(x => x.Items).HasField("_items");
    }
}

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> entity)
    {
        entity.ToTable("OrderItems");

        // OrderItem has no Id in the domain — use a shadow key
        entity.Property<int>("Id").ValueGeneratedOnAdd();
        entity.HasKey("Id");

        entity.Property(x => x.ProductId).IsRequired().HasMaxLength(36);
        entity.Property(x => x.Quantity).IsRequired();
        entity.Property(x => x.ReturnedQuantity);

        entity.OwnsOne(x => x.Price, money =>
        {
            money.Property(m => m.Amount).HasColumnName("Price_Amount").HasColumnType("decimal(18,2)");
            money.Property(m => m.Currency).HasColumnName("Price_Currency").HasMaxLength(10);
        });
    }
}
