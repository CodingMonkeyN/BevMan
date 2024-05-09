using BevMan.Application.Common.Exceptions;
using BevMan.Application.Common.Interfaces;

namespace BevMan.Application.User.Commands.UpdateUser;

public record UpdateUserCommand : IRequest<Guid>
{
    public Guid Id { get; set; }
    public string? DisplayName { get; set; }
}

public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
        RuleFor(v => v.DisplayName).MaximumLength(50);
    }
}

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    private readonly IUser _currentUser;

    public UpdateUserCommandHandler(IApplicationDbContext context, IUser currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<Guid> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        if (request.Id != Guid.Parse(_currentUser.Id!))
        {
            throw new ForbiddenAccessException();
        }

        Domain.Entities.User? user =
            await _context.Users.FirstOrDefaultAsync(user => user.Id == Guid.Parse(_currentUser.Id!),
                cancellationToken);
        Guard.Against.NotFound(Guid.Parse(_currentUser.Id!), user);
        user.DisplayName = request.DisplayName;
        await _context.SaveChangesAsync(cancellationToken);
        return user.Id;
    }
}
