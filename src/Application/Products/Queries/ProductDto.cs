using BevMan.Application.Common.Mappings;
using BevMan.Domain.Entities;

namespace BevMan.Application.Products.Queries;

public class ProductDto : IMapFrom<Product>
{
    public required string Name { get; set; }
    public required decimal Price { get; set; }
    public string? Description { get; set; }
}
