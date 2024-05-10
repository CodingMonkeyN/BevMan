using BevMan.Application.BalanceRequest.Commands.ApproveBalanceUpdate;
using BevMan.Application.BalanceRequest.Commands.CreateBalanceRequest;
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
            .MapPost(Approve, "{id}")
            .MapPost(CreateBalanceRequest);
    }

    private Task GetBalanceRequests(ISender sender)
    {
        return sender.Send(new GetBalanceRequestsQuery());
    }

    private Task<long> CreateBalanceRequest(ISender sender, [FromBody] CreateBalanceRequestCommand command)
    {
        return sender.Send(command);
    }

    private async Task<IResult> Approve(ISender sender, long id, [FromBody] ApproveBalanceUpdateCommand command)
    {
        if (id != command.BalanceRequestId)
        {
            return Results.BadRequest();
        }

        await sender.Send(command);
        return Results.NoContent();
    }
}
