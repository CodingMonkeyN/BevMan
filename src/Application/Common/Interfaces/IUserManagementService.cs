namespace BevMan.Application.Common.Interfaces;

public interface IUserManagementService
{
    Task DeleteUserAsync(string userId, CancellationToken cancellationToken);
}
