using BevMan.Application.User.Queries;
using BevMan.Application.User.Queries.GetUsers;
using BevMan.Web.Infrastructure;
using MediatR;

namespace BevMan.Web.Endpoints;

public class User : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .MapGet(GetUsers, role: BevMan.Infrastructure.Models.Role.UserManager);
    }

    private async Task<IEnumerable<UserDto>> GetUsers(ISender sender)
    {
        return await sender.Send(new GetUsersQuery());
    }
}
