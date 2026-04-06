using BizI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BizI.Infrastructure.Persistence.Configurations;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> entity)
    {
        entity.HasKey(x => x.Id);
        entity.Property(x => x.Id).HasMaxLength(36).IsRequired();

        entity.Property(x => x.Name).IsRequired().HasMaxLength(256);
        entity.Property(x => x.CustomerType).HasConversion<string>().HasMaxLength(50);
        entity.Property(x => x.LoyaltyPoints);
        entity.Property(x => x.LoyaltyTier).HasMaxLength(50);
        entity.Property(x => x.TotalSpent).HasColumnType("decimal(18,2)");
        entity.Property(x => x.TotalOrders);
        entity.Property(x => x.LastOrderDate);
        entity.Property(x => x.DebtTotal).HasColumnType("decimal(18,2)");
        entity.Property(x => x.DebtLimit).HasColumnType("decimal(18,2)");
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
