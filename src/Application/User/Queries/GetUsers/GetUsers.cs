using BevMan.Application.Common.Interfaces;
using BevMan.Application.Common.Mappings;

namespace BevMan.Application.User.Queries.GetUsers;

public record GetUsersQuery : IRequest<IEnumerable<UserDto>>;

public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, IEnumerable<UserDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetUsersQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<UserDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        return await _context.Users
            .Include(user => user.Roles)
            .ProjectToListAsync<UserDto>(_mapper.ConfigurationProvider);
    }
}
