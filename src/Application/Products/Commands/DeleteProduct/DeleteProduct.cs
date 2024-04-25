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
            .FindAsync(new object[] { request.Id }, cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        _context.Products.Remove(entity);
        if (!string.IsNullOrEmpty(entity.ImagePath))
        {
            // TODO: FIND THE REASON WHY THE BLOB DOES NOT GET DELETED
            await _storageService.DeleteFileAsync("products", entity.ImagePath, cancellationToken);
        }

        await _context.SaveChangesAsync(cancellationToken);
    }
}
