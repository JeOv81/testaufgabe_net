using Application.DTOs;
using Application.Features.Categories.Queries;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using ProductsApi.Filters;

namespace ProductsApi.Endpoints.Category;

public class GetCategoryByIdEndpoint : IEndpoint
{
    public void Map(IEndpointRouteBuilder app)
    {
        app.MapGet("/categories/{id}", HandleAsync)
           .WithName("GetCategoryById")
           .WithTags("Categories")
           .AddEndpointFilter<ValidationFilter<GetCategoryByIdQuery>>()
           .Produces<CategoryDto>(StatusCodes.Status200OK)
           .Produces(StatusCodes.Status404NotFound);
    }

    public static async Task<IResult> HandleAsync(
        [AsParameters] GetCategoryByIdQuery query,
        IQueryHandler<GetCategoryByIdQuery, CategoryDto?> handler,
        CancellationToken ct)
    {
        var categoryDto = await handler.Handle(query, ct);
        if (categoryDto == null)
        {
            return Results.NotFound($"Category with ID '{query.Id}' not found.");
        }
        return Results.Ok(categoryDto);
    }
}