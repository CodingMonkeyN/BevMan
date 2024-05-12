using BevMan.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BevMan.Infrastructure.Data.Configurations;

public class UserProfileConfiguration : IEntityTypeConfiguration<UserProfile>
{
    public void Configure(EntityTypeBuilder<UserProfile> builder)
    {
        builder.HasKey(p => p.Id);
        builder.HasOne(b => b.User);
        builder.Property(p => p.DisplayName)
            .IsRequired()
            .HasMaxLength(100);
        builder.HasOne(p => p.StorageObject).WithOne().HasForeignKey<UserProfile>(p => p.StorageObjectId);
        builder.Property(p => p.AvatarUrl)
            .HasMaxLength(255);
        builder.HasIndex(p => p.UserId).IsUnique();
    }
}
