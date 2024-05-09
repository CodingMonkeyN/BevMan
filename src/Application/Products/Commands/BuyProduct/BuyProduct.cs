using BevMan.Application.Common.Interfaces;
using BevMan.Domain.Entities;

namespace BevMan.Application.Products.Commands.BuyProduct;

public record BuyProductCommand : IRequest<BuyProductResponse>
{
    public long ProductId { get; set; }
}

public record BuyProductResponse(string? ErrorCode);

public class BuyProductCommandValidator : AbstractValidator<BuyProductCommand>
{
    public BuyProductCommandValidator()
    {
        RuleFor(v => v.ProductId).NotEmpty();
    }
}

public class BuyProductCommandHandler : IRequestHandler<BuyProductCommand, BuyProductResponse>
{
    private readonly IApplicationDbContext _context;
    private readonly IUser _currentUser;

    public BuyProductCommandHandler(IApplicationDbContext context, IUser currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<BuyProductResponse> Handle(BuyProductCommand request, CancellationToken cancellationToken)
    {
        Product? entity = await _context.Products.FindAsync(request.ProductId);

        Guard.Against.NotFound(request.ProductId, entity);

        Domain.Entities.Balance? userBalance =
            await _context.Balances.FirstOrDefaultAsync(balance => balance.UserId.ToString() == _currentUser.Id,
                cancellationToken);

        Guard.Against.Null(userBalance);

        if (userBalance.Amount < entity.Price)
        {
            return new BuyProductResponse("INSUFFICIENT_FUNDS");
        }

        userBalance.Amount -= entity.Price;
        await _context.SaveChangesAsync(cancellationToken);

        return new BuyProductResponse(null);
    }
}
