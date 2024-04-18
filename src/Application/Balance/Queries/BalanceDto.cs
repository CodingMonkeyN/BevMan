using BevMan.Application.Common.Mappings;

namespace BevMan.Application.Balance.Queries.GetBalance;

public class BalanceDto : IMapFrom<Domain.Entities.Balance>
{
    public decimal Amount { get; init; }
}
