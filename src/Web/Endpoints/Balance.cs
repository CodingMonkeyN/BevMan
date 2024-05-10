using BevMan.Application.Balance.Queries;
using BevMan.Application.Balance.Queries.GetBalance;
using BevMan.Web.Infrastructure;
using MediatR;

namespace BevMan.Web.Endpoints;

public class Balance : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .MapGet(GetBalance);
    }

    private async Task<BalanceDto> GetBalance(ISender sender)
    {
        return await sender.Send(new GetBalanceQuery());
    }
}
