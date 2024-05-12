using System.Reflection;
using BevMan.Application.Common.Interfaces;
using BevMan.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BevMan.Infrastructure.Data;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
    public DbSet<User> Users => Set<User>();
    public DbSet<StorageObject> StorageObjects => Set<StorageObject>();
    public DbSet<Balance> Balances => Set<Balance>();
    public DbSet<BalanceRequest> BalanceRequests => Set<BalanceRequest>();
    public DbSet<UserRole> UserRoles => Set<UserRole>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<UserProfile> UserProfiles => Set<UserProfile>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.HasPostgresEnum<AppRole>("public");
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
