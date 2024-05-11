using BevMan.Application.Common.Mappings;
using BevMan.Application.User.Queries;

namespace BevMan.Application.BalanceRequest.Queries;

public class BalanceRequestDto : IMapFrom<Domain.Entities.BalanceRequest>
{
    public required long Id { get; set; }
    public required UserDto User { get; set; }
    public required decimal Amount { get; set; }
}
