using BizI.Domain.Entities;
using BizI.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BizI.Infrastructure.Persistence.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> entity)
    {
        entity.HasKey(x => x.Id);
        entity.Property(x => x.Id).HasMaxLength(36).IsRequired();

        entity.Property(x => x.Name).IsRequired().HasMaxLength(256);
        entity.Property(x => x.SKU).IsRequired().HasMaxLength(100);
        entity.HasIndex(x => x.SKU).IsUnique();

        entity.Property(x => x.Barcode).HasMaxLength(100);
        entity.HasIndex(x => x.Barcode);

        entity.Property(x => x.CategoryId).IsRequired().HasMaxLength(36);
        entity.Property(x => x.Unit).IsRequired().HasMaxLength(50);
        entity.Property(x => x.Description).HasMaxLength(2000);
        entity.Property(x => x.IsActive);
        entity.Property(x => x.CreatedAt);
        entity.Property(x => x.UpdatedAt);
        entity.Property(x => x.IsDeleted);

        // Money value objects — mapped as owned types (two columns each)
        entity.OwnsOne(x => x.CostPrice, money =>
        {
            money.Property(m => m.Amount).HasColumnName("CostPrice_Amount").HasColumnType("decimal(18,2)");
            money.Property(m => m.Currency).HasColumnName("CostPrice_Currency").HasMaxLength(10);
        });

        entity.OwnsOne(x => x.SalePrice, money =>
        {
            money.Property(m => m.Amount).HasColumnName("SalePrice_Amount").HasColumnType("decimal(18,2)");
            money.Property(m => m.Currency).HasColumnName("SalePrice_Currency").HasMaxLength(10);
        });
    }
}
