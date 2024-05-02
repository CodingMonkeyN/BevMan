using BevMan.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BevMan.Infrastructure.Data.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(100);
        builder.HasOne(p => p.StorageObject).WithOne().HasForeignKey<Product>(p => p.StorageObjectId);
        builder.Property(p => p.Description)
            .HasMaxLength(500);
        builder.Property(p => p.PublicUrl)
            .HasMaxLength(255);
    }
}
