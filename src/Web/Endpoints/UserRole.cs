using BevMan.Application.UserRole.Commands.UpdateUserRoles;
using BevMan.Web.Infrastructure;
using MediatR;

namespace BevMan.Web.Endpoints;

public class UserRole : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .MapPut(UpdateUserRoles, "{id}", BevMan.Infrastructure.Models.Role.UserManager);
    }

    private async Task<IResult> UpdateUserRoles(ISender sender, Guid id, UpdateUserRolesCommand command)
    {
        if (id != command.UserId)
        {
            return Results.BadRequest();
        }

        await sender.Send(command);
        return Results.NoContent();
    }
}
