using BevMan.Domain.Entities;

namespace BevMan.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Product> Products { get; }
    DbSet<StorageObject> StorageObjects { get; }
    DbSet<Domain.Entities.Balance> Balances { get; }
    DbSet<Domain.Entities.BalanceRequest> BalanceRequests { get; }
    DbSet<Domain.Entities.UserRole> UserRoles { get; }
    DbSet<Domain.Entities.User> Users { get; }
    DbSet<Domain.Entities.UserProfile> UserProfiles { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
