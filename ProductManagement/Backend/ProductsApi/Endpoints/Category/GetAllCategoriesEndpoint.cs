using Application.DTOs;
using Application.Features.Categories.Queries;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using ProductsApi.Filters;

namespace ProductsApi.Endpoints.Category;

public class GetAllCategoriesEndpoint : IEndpoint
{
    public void Map(IEndpointRouteBuilder app)
    {
        app.MapGet("/categories", HandleAsync)
           .WithName("GetAllCategories")
           .WithTags("Categories")
           .AddEndpointFilter<ValidationFilter<GetAllCategoriesQuery>>()
           .Produces<ICollection<CategoryDto>>(StatusCodes.Status200OK);
    }

    public static async Task<IResult> HandleAsync(
        [AsParameters] GetAllCategoriesQuery query, 
        IQueryHandler<GetAllCategoriesQuery, ICollection<CategoryDto>> handler,
        CancellationToken ct)
    {
        var categories = await handler.Handle(query, ct);
        return Results.Ok(categories);
    }
}