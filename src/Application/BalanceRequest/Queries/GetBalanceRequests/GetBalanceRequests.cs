using BevMan.Application.Common.Interfaces;
using BevMan.Application.Common.Mappings;

namespace BevMan.Application.BalanceRequest.Queries.GetBalanceRequests;

public record GetBalanceRequestsQuery : IRequest<IEnumerable<BalanceRequestDto>>;

public class GetBalanceRequestsQueryHandler : IRequestHandler<GetBalanceRequestsQuery, IEnumerable<BalanceRequestDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUser _currentCurrentUser;
    private readonly IMapper _mapper;

    public GetBalanceRequestsQueryHandler(IApplicationDbContext context, IMapper mapper,
        ICurrentUser currentCurrentUser)
    {
        _context = context;
        _mapper = mapper;
        _currentCurrentUser = currentCurrentUser;
    }

    public async Task<IEnumerable<BalanceRequestDto>> Handle(GetBalanceRequestsQuery request,
        CancellationToken cancellationToken)
    {
        return await _context.BalanceRequests
            .Where(balanceRequest => Guid.Parse(_currentCurrentUser.Id!) != balanceRequest.UserId)
            .ProjectToListAsync<BalanceRequestDto>(_mapper.ConfigurationProvider);
    }
}
