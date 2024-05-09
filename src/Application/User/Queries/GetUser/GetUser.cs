using BevMan.Application.Common.Interfaces;

namespace BevMan.Application.User.Queries.GetUser;

public record GetUserQuery(Guid Id) : IRequest<UserDto>;

public class GetUserQueryHandler : IRequestHandler<GetUserQuery, UserDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetUserQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<UserDto> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        Domain.Entities.User? user = await _context.Users.Include(user => user.Roles).FirstOrDefaultAsync(
            user => user.Id == request.Id,
            cancellationToken);
        Guard.Against.NotFound(request.Id, user);

        return _mapper.Map<UserDto>(user);
    }
}
