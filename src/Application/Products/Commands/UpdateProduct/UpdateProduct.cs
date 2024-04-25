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
        Product? entity = await _context.Products.FindAsync(new object[] { request.Id }, cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        entity.Name = request.Name;
        entity.Description = request.Description;
        entity.Price = request.Price;

        await _context.SaveChangesAsync(cancellationToken);

        if (request.Image is null)
        {
            // TODO: Delete Image
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
