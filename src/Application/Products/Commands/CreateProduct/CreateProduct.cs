using BevMan.Application.Common.Interfaces;
using BevMan.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace BevMan.Application.Products.Commands.CreateProduct;

public record CreateProductCommand : IRequest<long>
{
    public required string Name { get; init; }
    public required decimal Price { get; init; }
    public string? Description { get; init; }
    public IFormFile? Image { get; init; }
}

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(command => command.Name).NotEmpty().MaximumLength(100);
        RuleFor(command => command.Price).NotNull().GreaterThan(0);
        RuleFor(command => command.Description).MaximumLength(500);
    }
}

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, long>
{
    private readonly IApplicationDbContext _context;
    private readonly IStorageService _storageService;

    public CreateProductCommandHandler(IApplicationDbContext context, IStorageService storageService)
    {
        _context = context;
        _storageService = storageService;
    }

    public async Task<long> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        Product entity = new() { Name = request.Name, Price = request.Price, Description = request.Description };

        _context.Products.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        if (request.Image is null)
        {
            return entity.Id;
        }

        Blob blob = new(request.Image);
        string path = blob.Name;
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
