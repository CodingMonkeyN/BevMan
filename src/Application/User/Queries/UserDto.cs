using BevMan.Application.Common.Mappings;

namespace BevMan.Application.User.Queries;

public class UserDto : IMapFrom<Domain.Entities.User>
{
    public Guid Id { get; init; }
    public required string Email { get; init; }
    public required string[] Roles { get; init; }

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
