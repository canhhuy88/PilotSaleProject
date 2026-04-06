using BizI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BizI.Infrastructure.Persistence.Configurations;

public class InventoryConfiguration : IEntityTypeConfiguration<Inventory>
{
    public void Configure(EntityTypeBuilder<Inventory> entity)
    {
        entity.HasKey(x => x.Id);
        entity.Property(x => x.Id).IsRequired();

        entity.Property(x => x.ProductId).IsRequired().HasMaxLength(36);
        entity.Property(x => x.WarehouseId).IsRequired().HasMaxLength(36);
        entity.Property(x => x.Quantity).IsRequired();
        entity.Property(x => x.CreatedAt);
        entity.Property(x => x.UpdatedAt);
        entity.Property(x => x.IsDeleted);

        // Composite index for fast product+warehouse lookups
        entity.HasIndex(x => new { x.ProductId, x.WarehouseId });
    }
}
