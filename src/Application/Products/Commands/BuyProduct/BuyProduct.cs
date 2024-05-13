using BevMan.Application.Common.Interfaces;
using BevMan.Domain.Entities;
using BevMan.Domain.Exceptions;

namespace BevMan.Application.Products.Commands.BuyProduct;

public record BuyProductCommand : IRequest
{
    public long ProductId { get; set; }
}

public class BuyProductCommandValidator : AbstractValidator<BuyProductCommand>
{
    public BuyProductCommandValidator()
    {
        RuleFor(v => v.ProductId).NotEmpty();
    }
}

public class BuyProductCommandHandler : IRequestHandler<BuyProductCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUser _currentCurrentUser;

    public BuyProductCommandHandler(IApplicationDbContext context, ICurrentUser currentCurrentUser)
    {
        _context = context;
        _currentCurrentUser = currentCurrentUser;
    }

    public async Task Handle(BuyProductCommand request, CancellationToken cancellationToken)
    {
        Product? entity = await _context.Products.FindAsync(request.ProductId);

        Guard.Against.NotFound(request.ProductId, entity);

        Domain.Entities.Balance? userBalance =
            await _context.Balances.FirstOrDefaultAsync(balance => balance.UserId.ToString() == _currentCurrentUser.Id,
                cancellationToken);

        Guard.Against.Null(userBalance);

        if (userBalance.Amount < entity.Price)
        {
            throw new BadRequestException("Not enough money");
        }

        userBalance.Amount -= entity.Price;
        await _context.SaveChangesAsync(cancellationToken);
    }
}
