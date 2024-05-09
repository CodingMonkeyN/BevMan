using BevMan.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BevMan.Infrastructure.Data.Configurations;

public class BalanceRequestConfiguration : IEntityTypeConfiguration<BalanceRequest>
{
    public void Configure(EntityTypeBuilder<BalanceRequest> builder)
    {
        builder.HasKey(b => b.Id);
        builder.HasOne(b => b.User);
    }
}
