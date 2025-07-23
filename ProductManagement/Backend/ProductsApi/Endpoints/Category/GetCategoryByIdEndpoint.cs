using Application.DTOs;
using Application.Features.Categories.Queries;
using Application.Interfaces;

namespace ProductsApi.Endpoints.Category;

public class GetCategoryByIdEndpoint : IEndpoint
{
    public void Map(IEndpointRouteBuilder app)
    {
        app.MapGet("/categories/{id}", HandleAsync)
           .WithName("GetCategoryById")
           .WithTags("Categories")
           .Produces<CategoryDto>(StatusCodes.Status200OK)
           .Produces(StatusCodes.Status404NotFound);
    }

    public static async Task<IResult> HandleAsync(
        Guid id,
        IQueryHandler<GetCategoryByIdQuery, CategoryDto?> handler,
        CancellationToken ct)
    {
        var categoryDto = await handler.Handle(new GetCategoryByIdQuery(id), ct);
        if (categoryDto == null)
        {
            return Results.NotFound($"Category with ID '{id}' not found.");
        }
        return Results.Ok(categoryDto);
    }
}