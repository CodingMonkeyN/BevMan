using BevMan.Application.Common.Interfaces;

namespace BevMan.Application.Balance.Queries.GetBalance;

public record GetBalanceQuery : IRequest<BalanceDto>;

public class GetBalanceQueryHandler : IRequestHandler<GetBalanceQuery, BalanceDto>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUser _currentUser;
    private readonly IMapper _mapper;

    public GetBalanceQueryHandler(IApplicationDbContext context, ICurrentUser currentUser, IMapper mapper)
    {
        _context = context;
        _currentUser = currentUser;
        _mapper = mapper;
    }

    public async Task<BalanceDto> Handle(GetBalanceQuery request, CancellationToken cancellationToken)
    {
        Guid userId = Guid.Parse(_currentUser.Id!);
        Domain.Entities.Balance? balance = await _context.Balances
            .FirstOrDefaultAsync(balance => balance.UserId == userId, cancellationToken);
        List<Domain.Entities.BalanceRequest> balanceRequest = await _context.BalanceRequests
            .Where(balanceRequests => balanceRequests.UserId == userId)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
        decimal amountInApproval = balanceRequest.Select(x => x.Amount).Sum();
        BalanceDto? mapped = balance is null
            ? new BalanceDto { Amount = 0, AmountInApproval = amountInApproval }
            : _mapper.Map<BalanceDto>(balance);
        mapped.AmountInApproval = amountInApproval;
        return mapped;
    }
}
