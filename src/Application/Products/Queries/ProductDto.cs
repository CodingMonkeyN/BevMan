using BevMan.Application.Common.Mappings;
using BevMan.Domain.Entities;

namespace BevMan.Application.Products.Queries;

public class ProductDto : IMapFrom<Product>
{
    public required long Id { get; init; }
    public required string Name { get; init; }
    public required decimal Price { get; init; }
    public string? Description { get; init; }
}
