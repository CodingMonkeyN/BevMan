using BevMan.Application.Common.Interfaces;

namespace BevMan.Application.UserRole.Commands.UpdateUserRoles;

public record UpdateUserRolesCommand : IRequest
{
    public required Guid UserId { get; init; }
    public required List<string> Roles { get; init; }
}

public class UpdateUserRolesCommandHandler : IRequestHandler<UpdateUserRolesCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateUserRolesCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(UpdateUserRolesCommand request, CancellationToken cancellationToken)
    {
        Domain.Entities.User? user = await _context.Users
            .FirstOrDefaultAsync(user => user.Id == request.UserId, cancellationToken);
        Guard.Against.Null(user);

        List<Domain.Entities.UserRole> currentRoles = await _context.UserRoles
            .Where(x => x.UserId == request.UserId)
            .ToListAsync(cancellationToken);
        List<Domain.Entities.UserRole> rolesToRemove = currentRoles
            .Where(x => !request.Roles.Contains(x.Role.ToString()))
            .ToList();
        List<string> rolesToAdd = request.Roles
            .Where(x => currentRoles.All(y => y.Role.ToString() != x))
            .ToList();
        _context.UserRoles.AddRange(rolesToAdd.Select(role => new Domain.Entities.UserRole
        {
            UserId = request.UserId, Role = Enum.Parse<Domain.Entities.AppRole>(role)
        }));
        _context.UserRoles.RemoveRange(rolesToRemove);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
