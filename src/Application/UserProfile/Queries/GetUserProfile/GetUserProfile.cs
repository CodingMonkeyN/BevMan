using BevMan.Application.Common.Interfaces;

namespace BevMan.Application.UserProfile.Queries.GetUserProfile;

public record GetUserProfileQuery : IRequest<UserProfileDto?>;

public class GetUserProfileQueryHandler : IRequestHandler<GetUserProfileQuery, UserProfileDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUser _currentUser;
    private readonly IMapper _mapper;

    public GetUserProfileQueryHandler(IApplicationDbContext context, ICurrentUser currentUser, IMapper mapper)
    {
        _context = context;
        _currentUser = currentUser;
        _mapper = mapper;
    }

    public async Task<UserProfileDto?> Handle(GetUserProfileQuery request, CancellationToken cancellationToken)
    {
        Guid userId = Guid.Parse(_currentUser.Id!);
        Domain.Entities.UserProfile? userProfile =
            await _context.UserProfiles.FirstOrDefaultAsync(userProfile => userProfile.UserId == userId,
                cancellationToken);
        return _mapper.Map<UserProfileDto?>(userProfile);
    }
}
