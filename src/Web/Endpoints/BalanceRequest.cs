using BevMan.Application.BalanceRequest.Commands.ApproveBalanceUpdate;
using BevMan.Application.BalanceRequest.Queries.GetBalanceRequests;
using BevMan.Web.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BevMan.Web.Endpoints;

public class BalanceRequest : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .MapGet(GetBalanceRequests)
            .MapPost(Approve);
    }

    private Task GetBalanceRequests(ISender sender)
    {
        return sender.Send(new GetBalanceRequestsQuery());
    }

    private Task Approve(ISender sender, [FromBody] ApproveBalanceUpdateCommand command)
    {
        return sender.Send(command);
    }
}
