using BevMan.Application.Common.Interfaces;
using BevMan.Domain.Entities;

namespace BevMan.Application.Products.Commands.CreateProduct;

public record CreateProductCommand : IRequest<long>
{
    public required string Name { get; init; }
    public required decimal Price { get; init; }
    public string? Description { get; init; }
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

    public CreateProductCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<long> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        Product entity = new Product { Name = request.Name, Price = request.Price, Description = request.Description };

        _context.Products.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
