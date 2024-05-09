using BevMan.Application.Common.Mappings;

namespace BevMan.Application.Balance.Queries;

public class BalanceDto : IMapFrom<Domain.Entities.Balance>
{
    public decimal Amount { get; init; }
}
