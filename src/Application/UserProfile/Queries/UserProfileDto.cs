using BevMan.Application.Common.Mappings;

namespace BevMan.Application.UserProfile.Queries;

public class UserProfileDto : IMapFrom<Domain.Entities.UserProfile>
{
    public required string DisplayName { get; set; }
    public string? AvatarUrl { get; set; }
}
