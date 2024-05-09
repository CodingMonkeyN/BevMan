using BevMan.Application.Common.Interfaces;
using BevMan.Domain.Entities;

namespace BevMan.Application.Products.Commands.DeleteProductImage;

public record DeleteProductImageCommand : IRequest<long>
{
    public required long ProductId { get; set; }
}

public class DeleteProductImageCommandValidator : AbstractValidator<DeleteProductImageCommand>
{
    public DeleteProductImageCommandValidator()
    {
        RuleFor(v => v.ProductId).NotEmpty();
    }
}

public class DeleteProductImageCommandHandler : IRequestHandler<DeleteProductImageCommand, long>
{
    private readonly IApplicationDbContext _context;
    private readonly IStorageService _storageService;

    public DeleteProductImageCommandHandler(IApplicationDbContext context, IStorageService storageService)
    {
        _context = context;
        _storageService = storageService;
    }

    public async Task<long> Handle(DeleteProductImageCommand request, CancellationToken cancellationToken)
    {
        Product? entity = await _context.Products
            .Include(p => p.StorageObject)
            .FirstOrDefaultAsync(p => p.Id == request.ProductId, cancellationToken);

        Guard.Against.NotFound(request.ProductId, entity);
        if (entity.StorageObject is null)
        {
            return entity.Id;
        }

        await _storageService.DeleteFileAsync("products", string.Join("/", entity.StorageObject.PathTokens),
            cancellationToken);
        return entity.Id;
    }
}
