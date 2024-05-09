using BevMan.Application.User.Commands.DeleteUser;
using BevMan.Application.User.Commands.UpdateUser;
using BevMan.Application.User.Queries;
using BevMan.Application.User.Queries.GetUser;
using BevMan.Application.User.Queries.GetUsers;
using BevMan.Web.Infrastructure;
using MediatR;

namespace BevMan.Web.Endpoints;

public class User : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .MapGet(GetUsers, role: BevMan.Infrastructure.Models.Role.Admin)
            .MapGet(GetUser, "{id}", BevMan.Infrastructure.Models.Role.Admin)
            .MapDelete(DeleteUser, "{id}")
            .MapPut(UpdateUser, "{id}");
    }

    private async Task<IEnumerable<UserDto>> GetUsers(ISender sender)
    {
        return await sender.Send(new GetUsersQuery());
    }

    private async Task<UserDto> GetUser(ISender sender, Guid id)
    {
        return await sender.Send(new GetUserQuery(id));
    }

    private async Task<IResult> UpdateUser(ISender sender, Guid id, UpdateUserCommand command)
    {
        await sender.Send(command);
        return Results.NoContent();
    }

    private async Task<IResult> DeleteUser(ISender sender, Guid id)
    {
        await sender.Send(new DeleteUserCommand());
        return Results.NoContent();
    }
}
