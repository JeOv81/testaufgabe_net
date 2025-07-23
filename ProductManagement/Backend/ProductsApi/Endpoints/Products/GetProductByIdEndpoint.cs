using Application.DTOs;
using Application.Features.Products.Queries;
using Application.Interfaces;

namespace ProductsApi.Endpoints.Products;

public class GetProductByIdEndpoint : IEndpoint
{
    public void Map(IEndpointRouteBuilder app)
    {
        app.MapGet("/products/{id}", HandleAsync)
           .WithName("GetProductById")
           .WithTags("Products")
           .Produces<ProductDto>(StatusCodes.Status200OK)
           .Produces(StatusCodes.Status404NotFound);
    }

    public static async Task<IResult> HandleAsync(
        Guid id,
        IQueryHandler<GetProductByIdQuery, ProductDto?> handler,
        CancellationToken ct)
    {
        var productDto = await handler.Handle(new GetProductByIdQuery(id), ct);
        if (productDto == null)
        {
            return Results.NotFound($"Product with ID '{id}' not found.");
        }
        return Results.Ok(productDto);
    }
}