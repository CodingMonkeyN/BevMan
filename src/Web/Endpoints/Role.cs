using BevMan.Application.AppRole.Queries.GetAppRoles;
using BevMan.Web.Infrastructure;
using MediatR;

namespace BevMan.Web.Endpoints;

public class Role : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .MapGet(GetRoles, role: BevMan.Infrastructure.Models.Role.Admin);
    }

    private async Task<string[]> GetRoles(ISender sender)
    {
        return await sender.Send(new GetAppRolesQuery());
    }
}
