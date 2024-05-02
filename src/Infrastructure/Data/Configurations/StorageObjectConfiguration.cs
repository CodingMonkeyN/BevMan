using BevMan.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BevMan.Infrastructure.Data.Configurations;

public class StorageObjectConfiguration : IEntityTypeConfiguration<StorageObject>
{
    public void Configure(EntityTypeBuilder<StorageObject> builder)
    {
        builder.HasKey(p => p.Id);
        builder.ToTable("objects", "storage", entityTypeBuilder =>
        {
            entityTypeBuilder.ExcludeFromMigrations();
        });
    }
}
