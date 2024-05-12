using CleanArchitecture.Application.Common.Interfaces;

namespace CleanArchitecture.Application.UserProfile.Queries.GetUserProfile;

public record GetUserProfileQuery : IRequest<UserProfileDto>
{
}

public class GetUserProfileQueryValidator : AbstractValidator<GetUserProfileQuery>
{
    public GetUserProfileQueryValidator()
    {
    }
}

public class GetUserProfileQueryHandler : IRequestHandler<GetUserProfileQuery, UserProfileDto>
{
    private readonly IApplicationDbContext _context;

    public GetUserProfileQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<UserProfileDto> Handle(GetUserProfileQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
