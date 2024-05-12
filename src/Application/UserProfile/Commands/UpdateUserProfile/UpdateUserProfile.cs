using BevMan.Application.Common.Interfaces;

namespace BevMan.Application.UserProfile.Commands.UpdateUserProfile;

public record UpdateUserProfileCommand : IRequest<long>
{
    public required string DisplayName { get; init; }
}

public class UpdateUserProfileCommandValidator : AbstractValidator<UpdateUserProfileCommand>
{
    public UpdateUserProfileCommandValidator()
    {
        RuleFor(v => v.DisplayName)
            .MaximumLength(100)
            .NotNull()
            .NotEmpty();
    }
}

public class UpdateUserProfileCommandHandler : IRequestHandler<UpdateUserProfileCommand, long>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUser _currentCurrentUser;

    public UpdateUserProfileCommandHandler(IApplicationDbContext context, ICurrentUser currentCurrentUser)
    {
        _context = context;
        _currentCurrentUser = currentCurrentUser;
    }

    public async Task<long> Handle(UpdateUserProfileCommand request, CancellationToken cancellationToken)
    {
        Guid userId = Guid.Parse(_currentCurrentUser.Id!);
        Domain.Entities.UserProfile? userProfile =
            await _context.UserProfiles.FirstOrDefaultAsync(userProfile => userProfile.UserId == userId,
                cancellationToken);
        if (userProfile is null)
        {
            userProfile = _context.UserProfiles
                .Add(new Domain.Entities.UserProfile { DisplayName = request.DisplayName, UserId = userId }).Entity;
        }

        userProfile.DisplayName = request.DisplayName;
        await _context.SaveChangesAsync(cancellationToken);
        return userProfile.Id;
    }
}
