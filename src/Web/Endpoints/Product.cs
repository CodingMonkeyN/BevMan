using BevMan.Application.Products.Commands.CreateProduct;
using BevMan.Application.Products.Commands.DeleteProduct;
using BevMan.Application.Products.Commands.UpdateProduct;
using BevMan.Application.Products.Queries;
using BevMan.Application.Products.Queries.GetProduct;
using BevMan.Application.Products.Queries.GetProducts;
using BevMan.Web.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BevMan.Web.Endpoints;

public class Product : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .DisableAntiforgery()
            .MapGet(GetProducts)
            .MapGet(GetProduct, "{id}")
            .MapPost(CreateProduct, role: BevMan.Infrastructure.Models.Role.Admin)
            .MapPut(UpdateProduct, "{id}", BevMan.Infrastructure.Models.Role.Admin)
            .MapDelete(DeleteTodoItem, "{id}", BevMan.Infrastructure.Models.Role.Admin);
    }

    private Task<long> CreateProduct(ISender sender, [FromForm] CreateProductCommand command)
    {
        return sender.Send(command);
    }

    private async Task<IResult> UpdateProduct(ISender sender, long id, [FromForm] UpdateProductCommand command)
    {
        if (id != command.Id)
        {
            return Results.BadRequest();
        }

        await sender.Send(command);
        return Results.NoContent();
    }

    private async Task<IResult> DeleteTodoItem(ISender sender, long id)
    {
        await sender.Send(new DeleteProductCommand(id));
        return Results.NoContent();
    }

    private async Task<IEnumerable<ProductDto>> GetProducts(ISender sender)
    {
        return await sender.Send(new GetProductsQuery());
    }

    private async Task<ProductDto> GetProduct(ISender sender, long id)
    {
        return await sender.Send(new GetProductQuery(id));
    }
}
