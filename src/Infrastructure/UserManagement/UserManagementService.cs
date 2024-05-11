using BevMan.Application.Common.Interfaces;
using Microsoft.Extensions.Options;
using Supabase;
using SupabaseOptions = BevMan.Infrastructure.Models.SupabaseOptions;

namespace BevMan.Infrastructure.UserManagement;

public class UserManagementService : IUserManagementService
{
    private readonly Client _client;
    private readonly IOptions<SupabaseOptions> _options;

    public UserManagementService(Client client, IOptions<SupabaseOptions> options)
    {
        _client = client;
        _options = options;
    }

    public async Task DeleteUserAsync(string userId, CancellationToken cancellationToken)
    {
        bool result = await _client.AdminAuth(_options.Value.ApiKey).DeleteUser(userId);
    }
}
