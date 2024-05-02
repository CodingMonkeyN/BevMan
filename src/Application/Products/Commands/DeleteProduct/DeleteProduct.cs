using BevMan.Application.Common.Interfaces;
using BevMan.Domain.Entities;

namespace BevMan.Application.Products.Commands.DeleteProduct;

public record DeleteProductCommand(long Id) : IRequest;

public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly IStorageService _storageService;

    public DeleteProductCommandHandler(IApplicationDbContext context, IStorageService storageService)
    {
        _context = context;
        _storageService = storageService;
    }

    public async Task Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        Product? entity = await _context.Products
            .Include(product => product.StorageObject)
            .FirstOrDefaultAsync(product => product.Id == request.Id, cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        StorageObject? storageObject = entity.StorageObject;
        _context.Products.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
        if (storageObject is not null)
        {
            await _storageService.DeleteFileAsync("products", string.Join("/", storageObject.PathTokens),
                cancellationToken);
        }
    }
}
