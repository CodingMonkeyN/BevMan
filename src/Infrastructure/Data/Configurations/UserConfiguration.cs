using BevMan.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BevMan.Infrastructure.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(user => user.Id).HasColumnName("id");
        builder.Property(user => user.Email).HasColumnName("email");
        builder.HasKey(user => user.Id);
        builder.ToTable("users", "auth", entityTypeBuilder =>
        {
            entityTypeBuilder.ExcludeFromMigrations();
        });
    }
}
