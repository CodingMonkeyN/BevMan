using BevMan.Application.Common.Interfaces;

namespace BevMan.Application.Balance.Commands.UpdateBalance;

public record UpdateBalanceCommand : IRequest<long>
{
    public required decimal Amount { get; init; }
}

public class UpdateBalanceCommandValidator : AbstractValidator<UpdateBalanceCommand>
{
    public UpdateBalanceCommandValidator()
    {
        RuleFor(e => e.Amount).NotNull().GreaterThanOrEqualTo(0);
    }
}

public class UpdateBalanceCommandHandler : IRequestHandler<UpdateBalanceCommand, long>
{
    private readonly IApplicationDbContext _context;
    private readonly IUser _user;

    public UpdateBalanceCommandHandler(IApplicationDbContext context, IUser user)
    {
        _context = context;
        _user = user;
    }

    public async Task<long> Handle(UpdateBalanceCommand request, CancellationToken cancellationToken)
    {
        Domain.Entities.Balance balance =
            await _context.Balances
                .FirstOrDefaultAsync(balance => balance.UserId == Guid.Parse(_user.Id!),
                    cancellationToken)
            ?? _context.Balances
                .Add(new Domain.Entities.Balance { UserId = Guid.Parse(_user.Id!) }).Entity;

        balance.Amount = request.Amount;

        await _context.SaveChangesAsync(cancellationToken);

        return balance.Id;
    }
}
