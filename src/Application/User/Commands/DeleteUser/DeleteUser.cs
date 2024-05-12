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
    private readonly ICurrentUser _currentCurrentUser;
    private readonly IUserManagementService _userManagementService;

    public DeleteUserCommandHandler(IApplicationDbContext context, ICurrentUser currentCurrentUser,
        IUserManagementService userManagementService)
    {
        _context = context;
        _currentCurrentUser = currentCurrentUser;
        _userManagementService = userManagementService;
    }

    public async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        if (request.Id != Guid.Parse(_currentCurrentUser.Id!))
        {
            throw new ForbiddenAccessException();
        }


        Domain.Entities.User? user =
            await _context.Users.FirstOrDefaultAsync(user => user.Id == Guid.Parse(_currentCurrentUser.Id!),
                cancellationToken);
        Guard.Against.NotFound(Guid.Parse(_currentCurrentUser.Id!), user);

        await _userManagementService.DeleteUserAsync(_currentCurrentUser.Id!, cancellationToken);
    }
}
