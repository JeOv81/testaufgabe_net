using Application.DTOs;
using Application.Features.Products.Queries;
using Application.Interfaces;
using Application.Resources;
using Microsoft.Extensions.Localization;
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
        IStringLocalizer<MessageTemplate> localizer,
        CancellationToken ct)
    {
        var productDto = await handler.Handle(query, ct);
        if (productDto == null)
        {
            return Results.NotFound(localizer["Template_Id_NotFound", "Product", query.Id].Value);
        }
        return Results.Ok(productDto);
    }
}