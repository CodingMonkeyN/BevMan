using BevMan.Application.Balance.Commands.UpdateBalance;
using BevMan.Application.Balance.Queries.GetBalance;
using BevMan.Web.Infrastructure;
using MediatR;

namespace BevMan.Web.Endpoints;

public class Balance : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .MapGet(GetBalance)
            .MapPut(UpdateBalance, "");
    }

    private async Task<BalanceDto> GetBalance(ISender sender)
    {
        return await sender.Send(new GetBalanceQuery());
    }

    private async Task<long> UpdateBalance(ISender sender, UpdateBalanceCommand command)
    {
        return await sender.Send(command);
    }
}
