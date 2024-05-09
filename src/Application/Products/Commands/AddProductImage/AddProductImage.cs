using BevMan.Application.Common.Interfaces;
using BevMan.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace BevMan.Application.Products.Commands.AddProductImage;

public record AddProductImageCommand : IRequest<long>
{
    public required long ProductId { get; init; }
    public required IFormFile Image { get; init; }
}

public class AddProductImageCommandValidator : AbstractValidator<AddProductImageCommand>
{
    public AddProductImageCommandValidator()
    {
        RuleFor(v => v.ProductId)
            .NotEmpty();
        RuleFor(v => v.Image)
            .NotEmpty();
    }
}

public class AddProductImageCommandHandler : IRequestHandler<AddProductImageCommand, long>
{
    private readonly IApplicationDbContext _context;
    private readonly IStorageService _storageService;

    public AddProductImageCommandHandler(IApplicationDbContext context, IStorageService storageService)
    {
        _context = context;
        _storageService = storageService;
    }

    public async Task<long> Handle(AddProductImageCommand request, CancellationToken cancellationToken)
    {
        Product? entity = await _context.Products
            .Where(product => product.Id == request.ProductId)
            .FirstOrDefaultAsync(cancellationToken);

        Guard.Against.NotFound(request.ProductId, entity);

        Blob blob = new(request.Image);
        string path = request.ProductId.ToString();
        string[] pathTokens = path.Split('/');
        string publicUrl = await _storageService.UploadFileAsync(blob, "products",
            path, cancellationToken);
        StorageObject? storageObject = await _context.StorageObjects
            .Where(storage => storage.BucketId == "products" && storage.PathTokens.Equals(pathTokens))
            .FirstOrDefaultAsync(cancellationToken);
        entity.StorageObject = storageObject;
        entity.PublicUrl = publicUrl;
        await _context.SaveChangesAsync(cancellationToken);
        return entity.Id;
    }
}
