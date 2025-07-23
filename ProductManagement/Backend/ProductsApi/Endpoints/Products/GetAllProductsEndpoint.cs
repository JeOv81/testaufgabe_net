using Application.DTOs;
using Application.Features.Products.Queries;
using Application.Interfaces;

namespace ProductsApi.Endpoints.Products;

public class GetAllProductsEndpoint : IEndpoint
{
    public void Map(IEndpointRouteBuilder app)
    {
        app.MapGet("/products", HandleAsync)
           .AllowAnonymous()
           .WithName("GetAllProducts")
           .WithTags("Products")
           .Produces<ICollection<ProductDto>>(StatusCodes.Status200OK);
    }

    public static async Task<IResult> HandleAsync(
        string? search, 
        int? pageNumber, 
        int? pageSize,  
        IQueryHandler<GetAllProductsQuery, ICollection<ProductDto>> handler,
        CancellationToken ct)
    {
        var query = new GetAllProductsQuery(
            SearchTerm: search,
            PageNumber: pageNumber ?? 1, 
            PageSize: pageSize ?? 10    
        );

        var products = await handler.Handle(query, ct);
        return Results.Ok(products);
    }
}