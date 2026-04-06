using BizI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BizI.Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> entity)
    {
        entity.HasKey(x => x.Id);
        entity.Property(x => x.Id).HasMaxLength(36).IsRequired();

        entity.Property(x => x.Username).IsRequired().HasMaxLength(100);
        entity.HasIndex(x => x.Username).IsUnique();

        entity.Property(x => x.PasswordHash).IsRequired().HasMaxLength(500);
        entity.Property(x => x.FullName).IsRequired().HasMaxLength(256);
        entity.Property(x => x.RoleId).IsRequired().HasMaxLength(36);
        entity.Property(x => x.BranchId).HasMaxLength(36);
        entity.Property(x => x.IsActive);
        entity.Property(x => x.CreatedAt);
        entity.Property(x => x.UpdatedAt);
        entity.Property(x => x.IsDeleted);
    }
}
