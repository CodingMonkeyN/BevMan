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
        (string imagePath, string publicUrl) = await _storageService.UploadFileAsync(blob, "products",
            $"{entity.Id}/{entity.Name}{Path.GetExtension(blob.FileName)}", cancellationToken);
        entity.ImagePath = imagePath;
        entity.PublicUrl = publicUrl;
        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
