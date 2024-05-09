using BevMan.Application.Common.Exceptions;
using BevMan.Application.Common.Interfaces;

namespace BevMan.Application.User.Commands.DeleteUser;

public record DeleteUserCommand : IRequest
{
    public Guid Id { get; set; }
}

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly IUser _currentUser;

    public DeleteUserCommandHandler(IApplicationDbContext context, IUser currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        if (request.Id != Guid.Parse(_currentUser.Id!))
        {
            throw new ForbiddenAccessException();
        }

        Domain.Entities.User? user =
            await _context.Users.FirstOrDefaultAsync(user => user.Id == Guid.Parse(_currentUser.Id!),
                cancellationToken);
        Guard.Against.NotFound(Guid.Parse(_currentUser.Id!), user);

        _context.Users.Remove(user);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
