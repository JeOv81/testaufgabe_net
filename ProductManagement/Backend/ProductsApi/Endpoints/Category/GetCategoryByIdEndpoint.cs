using Application.DTOs;
using Application.Features.Categories.Queries;
using Application.Interfaces;
using Application.Resources;
using Microsoft.Extensions.Localization;
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
        IStringLocalizer<MessageTemplate> localizer,
        CancellationToken ct)
    {
        var categoryDto = await handler.Handle(query, ct);
        if (categoryDto == null)
        {
            return Results.NotFound(localizer["Template_Id_NotFound", "Category", query.Id].Value);
        }
        return Results.Ok(categoryDto);
    }
}