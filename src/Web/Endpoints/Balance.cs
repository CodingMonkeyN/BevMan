using BevMan.Application.Balance.Commands.UpdateBalance;
using BevMan.Application.Balance.Queries;
using BevMan.Application.Balance.Queries.GetBalance;
using BevMan.Application.BalanceRequest.Commands.ApproveBalanceUpdate;
using BevMan.Web.Infrastructure;
using MediatR;

namespace BevMan.Web.Endpoints;

public class Balance : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .MapGet(GetBalance)
            .MapPost(UpdateBalance);
    }

    private async Task<BalanceDto> GetBalance(ISender sender)
    {
        return await sender.Send(new GetBalanceQuery());
    }

    private async Task<long> UpdateBalance(ISender sender, UpdateBalanceCommand command)
    {
        return await sender.Send(command);
    }

    private Task ApproveBalanceUpdate(ISender sender, ApproveBalanceUpdateCommand command)
    {
        return sender.Send(command);
    }
}
