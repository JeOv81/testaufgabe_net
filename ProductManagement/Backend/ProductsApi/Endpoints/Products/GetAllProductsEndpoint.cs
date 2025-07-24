using Application.DTOs;
using Application.Features.Products.Queries;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using ProductsApi.Filters;

namespace ProductsApi.Endpoints.Products;

public class GetAllProductsEndpoint : IEndpoint
{
    public void Map(IEndpointRouteBuilder app)
    {
        app.MapGet("/products", HandleAsync)
           .AllowAnonymous()
           .WithName("GetAllProducts")
           .WithTags("Products")
           .AddEndpointFilter<ValidationFilter<GetAllProductsQuery>>()
           .Produces<ICollection<ProductDto>>(StatusCodes.Status200OK);
    }

    public static async Task<IResult> HandleAsync(
        [AsParameters] GetAllProductsQuery query,
        IQueryHandler<GetAllProductsQuery, ICollection<ProductDto>> handler,
        CancellationToken ct)
    {
        var products = await handler.Handle(query, ct);
        return Results.Ok(products);
    }
}