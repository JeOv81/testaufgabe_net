using Application.DTOs;
using Application.Features.Products.Queries;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using ProductsApi.Filters;

namespace ProductsApi.Endpoints.Products;

public class GetProductByIdEndpoint : IEndpoint
{
    public void Map(IEndpointRouteBuilder app)
    {
        app.MapGet("/products/{id}", HandleAsync)
           .AllowAnonymous()
           .WithName("GetProductById")
           .WithTags("Products")
           .AddEndpointFilter<ValidationFilter<GetProductByIdQuery>>()
           .Produces<ProductDto>(StatusCodes.Status200OK)
           .Produces(StatusCodes.Status404NotFound);
    }

    public static async Task<IResult> HandleAsync(
       [AsParameters] GetProductByIdQuery query,
        IQueryHandler<GetProductByIdQuery, ProductDto?> handler,
        CancellationToken ct)
    {
        var productDto = await handler.Handle(query, ct);
        if (productDto == null)
        {
            return Results.NotFound($"Product with ID '{query.Id}' not found.");
        }
        return Results.Ok(productDto);
    }
}