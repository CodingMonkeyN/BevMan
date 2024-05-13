using BevMan.Application.Balance.Queries;
using BevMan.Application.Common.Mappings;
using BevMan.Application.UserProfile.Queries;

namespace BevMan.Application.User.Queries;

public class UserDto : IMapFrom<Domain.Entities.User>
{
    public Guid Id { get; init; }
    public required string Email { get; init; }
    public required string[] Roles { get; init; }
    public BalanceDto? Balance { get; init; }
    public UserProfileDto? Profile { get; init; }

    public void Mapping(Profile profile)
    {
        profile
            .CreateMap<Domain.Entities.User, UserDto>()
            .ForMember(
                userDto => userDto.Roles,
                options => options.MapFrom(
                    user => user.Roles.Select(role => role.Role.ToString())
                )
            );
    }
}
