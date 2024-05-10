using BevMan.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace BevMan.Application.BalanceRequest.Commands.CreateBalanceRequest;

public record CreateBalanceRequestCommand : IRequest<long>
{
    public required decimal Amount { get; init; }
}

public class CreateBalanceRequestCommandValidator : AbstractValidator<CreateBalanceRequestCommand>
{
    public CreateBalanceRequestCommandValidator()
    {
        RuleFor(e => e.Amount).NotNull().GreaterThanOrEqualTo(0);
    }
}

public class CreateBalanceRequestCommandHandler : IRequestHandler<CreateBalanceRequestCommand, long>
{
    private readonly IApplicationDbContext _context;
    private readonly IUser _user;

    public CreateBalanceRequestCommandHandler(IApplicationDbContext context, IUser user)
    {
        _context = context;
        _user = user;
    }

    public async Task<long> Handle(CreateBalanceRequestCommand request, CancellationToken cancellationToken)
    {
        EntityEntry<Domain.Entities.BalanceRequest> balanceRequest =
            _context.BalanceRequests.Add(
                new Domain.Entities.BalanceRequest { Amount = request.Amount, UserId = Guid.Parse(_user.Id!) });
        await _context.SaveChangesAsync(cancellationToken);
        return balanceRequest.Entity.Id;
    }
}
