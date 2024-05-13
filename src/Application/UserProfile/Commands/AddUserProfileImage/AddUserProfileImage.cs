using BevMan.Application.Common.Interfaces;
using BevMan.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace BevMan.Application.UserProfile.Commands.AddUserProfileImage;

public record AddUserProfileImageCommand : IRequest<long>
{
    public required IFormFile Image { get; init; }
}

public class AddUserProfileImageCommandValidator : AbstractValidator<AddUserProfileImageCommand>
{
    public AddUserProfileImageCommandValidator()
    {
        RuleFor(v => v.Image)
            .NotNull()
            .WithMessage("Image is required.");
    }
}

public class AddUserProfileImageCommandHandler : IRequestHandler<AddUserProfileImageCommand, long>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUser _currentCurrentUser;
    private readonly IStorageService _storageService;

    public AddUserProfileImageCommandHandler(IApplicationDbContext context, IStorageService storageService,
        ICurrentUser currentCurrentUser)
    {
        _context = context;
        _storageService = storageService;
        _currentCurrentUser = currentCurrentUser;
    }

    public async Task<long> Handle(AddUserProfileImageCommand request, CancellationToken cancellationToken)
    {
        Guid userId = Guid.Parse(_currentCurrentUser.Id!);
        Domain.Entities.UserProfile? userProfile = await _context.UserProfiles
            .Where(userProfile => userProfile.UserId == userId)
            .FirstOrDefaultAsync(cancellationToken);

        if (userProfile is null)
        {
            throw new Exception();
        }


        Blob blob = new(request.Image);
        long random = new Random().NextInt64();
        string path = $"{userId}-{random}";
        string[] pathTokens = path.Split('/');
        string publicUrl = await _storageService.UploadFileAsync(blob, "userProfiles",
            path, cancellationToken);
        StorageObject? storageObject = await _context.StorageObjects
            .Where(storage => storage.BucketId == "userProfiles" && storage.PathTokens.Equals(pathTokens))
            .FirstOrDefaultAsync(cancellationToken);
        userProfile.StorageObject = storageObject;
        userProfile.AvatarUrl = publicUrl;
        await _context.SaveChangesAsync(cancellationToken);
        return userProfile.Id;
    }
}
