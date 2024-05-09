using BevMan.Application.Common.Interfaces;

namespace BevMan.Application.BalanceRequest.Commands.ApproveBalanceUpdate;

public record ApproveBalanceUpdateCommand : IRequest
{
    public long BalanceRequestId { get; set; }
    public bool IsApproved { get; set; }
}

public class ApproveBalanceUpdateCommandValidator : AbstractValidator<ApproveBalanceUpdateCommand>
{
    public ApproveBalanceUpdateCommandValidator()
    {
        RuleFor(v => v.BalanceRequestId).NotEmpty();
    }
}

public class ApproveBalanceUpdateCommandHandler : IRequestHandler<ApproveBalanceUpdateCommand>
{
    private readonly IApplicationDbContext _context;

    public ApproveBalanceUpdateCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(ApproveBalanceUpdateCommand request, CancellationToken cancellationToken)
    {
        Domain.Entities.BalanceRequest? balanceRequest =
            await _context.BalanceRequests.FirstOrDefaultAsync(
                balance => balance.Id == request.BalanceRequestId,
                cancellationToken);
        Guard.Against.NotFound(request.BalanceRequestId, balanceRequest);

        if (request.IsApproved)
        {
            Domain.Entities.Balance userBalance =
                await _context.Balances.FirstAsync(balance => balance.UserId == balanceRequest.UserId,
                    cancellationToken);
            userBalance.Amount += balanceRequest.Amount;
        }

        _context.BalanceRequests.Remove(balanceRequest);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
