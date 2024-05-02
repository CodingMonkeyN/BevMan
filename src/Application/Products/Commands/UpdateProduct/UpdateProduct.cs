using BevMan.Application.Common.Interfaces;
using BevMan.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace BevMan.Application.Products.Commands.UpdateProduct;

public record UpdateProductCommand : IRequest<long>
{
    public required long Id { get; init; }
    public required string Name { get; init; }
    public string? Description { get; init; }
    public required decimal Price { get; init; }
    public IFormFile? Image { get; init; }
    public bool DeleteImage { get; init; }
}

public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(command => command.Name).NotEmpty().MaximumLength(100);
        RuleFor(command => command.Description).MaximumLength(500);
        RuleFor(command => command.Price).NotNull().GreaterThan(0);
    }
}

public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, long>
{
    private readonly IApplicationDbContext _context;
    private readonly IStorageService _storageService;

    public UpdateProductCommandHandler(IApplicationDbContext context, IStorageService storageService)
    {
        _context = context;
        _storageService = storageService;
    }

    public async Task<long> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        Product? entity = await _context.Products
            .Include(product => product.StorageObject)
            .FirstOrDefaultAsync(product => product.Id == request.Id, cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        entity.Name = request.Name;
        entity.Description = request.Description;
        entity.Price = request.Price;

        await _context.SaveChangesAsync(cancellationToken);

        StorageObject? currentStorageObject = entity.StorageObject;
        if (request.Image is not null)
        {
            Blob blob = new(request.Image);
            string path = blob.Name;
            string[] pathTokens = path.Split("/");
            string publicUrl = await _storageService.UploadFileAsync(blob, "products",
                path, cancellationToken);
            entity.PublicUrl = publicUrl;
            StorageObject? storageObject = await _context.StorageObjects
                .Where(storage => storage.BucketId == "products" && storage.PathTokens.Equals(pathTokens))
                .FirstOrDefaultAsync(cancellationToken);
            entity.StorageObject = storageObject;
            await _context.SaveChangesAsync(cancellationToken);
        }

        if ((request.Image is not null || request.DeleteImage) && currentStorageObject is not null)
        {
            await DeleteFileAsync(currentStorageObject, cancellationToken);
        }

        return entity.Id;
    }


    private async Task DeleteFileAsync(StorageObject storageObject,
        CancellationToken cancellationToken)
    {
        string bucketId = storageObject!.BucketId;
        string path = string.Join("/", storageObject.PathTokens);
        await _storageService.DeleteFileAsync(bucketId,
            path, cancellationToken);
    }
}
