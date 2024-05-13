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
    public required int Quantity { get; init; }
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

    public UpdateProductCommandHandler(IApplicationDbContext context)
    {
        _context = context;
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
        entity.Quantity = request.Quantity;

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
