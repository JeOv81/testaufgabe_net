using Application.DTOs;
using Application.Features.Categories.Queries;
using Application.Interfaces;

namespace ProductsApi.Endpoints.Category;

public class GetAllCategoriesEndpoint : IEndpoint
{
    public void Map(IEndpointRouteBuilder app)
    {
        app.MapGet("/categories", HandleAsync)
           .WithName("GetAllCategories")
           .WithTags("Categories")
           .Produces<ICollection<CategoryDto>>(StatusCodes.Status200OK);
    }

    public static async Task<IResult> HandleAsync(
        string? search, 
        IQueryHandler<GetAllCategoriesQuery, ICollection<CategoryDto>> handler,
        CancellationToken ct)
    {
        var query = new GetAllCategoriesQuery(SearchTerm: search);
        var categories = await handler.Handle(query, ct);
        return Results.Ok(categories);
    }
}