using BevMan.Application.UserProfile.Commands.AddUserProfileImage;
using BevMan.Application.UserProfile.Commands.UpdateUserProfile;
using BevMan.Application.UserProfile.Queries;
using BevMan.Application.UserProfile.Queries.GetUserProfile;
using BevMan.Web.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BevMan.Web.Endpoints;

public class UserProfile : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .DisableAntiforgery()
            .MapGet(GetUserProfile)
            .MapPost(AddUserProfileImage, "image")
            .MapPost(UpdateUserProfile);
    }

    private async Task<UserProfileDto?> GetUserProfile(ISender sender)
    {
        return await sender.Send(new GetUserProfileQuery());
    }

    private async Task<IResult> AddUserProfileImage(ISender sender, [FromForm] AddUserProfileImageCommand command)
    {
        await sender.Send(command);
        return Results.NoContent();
    }

    private async Task<IResult> UpdateUserProfile(ISender sender, [FromBody] UpdateUserProfileCommand command)
    {
        await sender.Send(command);
        return Results.NoContent();
    }
}
