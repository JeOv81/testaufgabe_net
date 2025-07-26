using Application.Features.Categories.Commands;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using ProductsApi.Filters;

namespace ProductsApi.Endpoints.Category;

public class CreateCategoryEndpoint : IEndpoint
{
    /// <inheritdoc />
    public void Map(IEndpointRouteBuilder app)
    {
        app.MapPost("/categories", HandleAsync)
           .WithName("CreateCategory")
           .WithTags("Categories")
           .AddEndpointFilter<ValidationFilter<CreateCategoryCommand>>()
           .Produces<Guid>(StatusCodes.Status201Created)
           .Produces(StatusCodes.Status400BadRequest);
    }

    public static async Task<IResult> HandleAsync(
        [FromBody] CreateCategoryCommand command,
        ICommandHandler<CreateCategoryCommand, Guid> handler,
        CancellationToken ct)
    {
        var categoryId = await handler.Handle(command, ct);
        return Results.Created($"/categories/{categoryId}", categoryId);
    }
}