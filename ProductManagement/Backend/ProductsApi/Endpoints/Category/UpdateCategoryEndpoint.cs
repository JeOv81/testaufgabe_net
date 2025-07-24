using Application.Features.Categories.Commands;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using ProductsApi.Filters;

namespace ProductsApi.Endpoints.Category;

public class UpdateCategoryEndpoint : IEndpoint
{
    public void Map(IEndpointRouteBuilder app)
    {
        app.MapPut("/categories", HandleAsync)
           .WithName("UpdateCategory")
           .WithTags("Categories")
           .AddEndpointFilter<ValidationFilter<UpdateCategoryCommand>>()
           .Produces(StatusCodes.Status200OK) 
           .Produces(StatusCodes.Status400BadRequest)
           .Produces(StatusCodes.Status404NotFound);
    }
    public static async Task<IResult> HandleAsync(
        [FromBody] UpdateCategoryCommand command,
        ICommandHandler<UpdateCategoryCommand, bool> handler, 
        CancellationToken ct)
    {
        var success = await handler.Handle(command, ct);
        if (!success)
        {
            return Results.NotFound($"Category with ID '{command.Id}' not found.");
        }
        return Results.Ok(); 
    }
}