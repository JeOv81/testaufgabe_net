using Application.Features.Categories.Commands;
using Application.Interfaces;

namespace ProductsApi.Endpoints.Category;

public class UpdateCategoryEndpoint : IEndpoint
{
    public void Map(IEndpointRouteBuilder app)
    {
        app.MapPut("/categories/{id}", HandleAsync)
           .WithName("UpdateCategory")
           .WithTags("Categories")
           .Produces(StatusCodes.Status200OK) 
           .Produces(StatusCodes.Status400BadRequest)
           .Produces(StatusCodes.Status404NotFound);
    }
    public static async Task<IResult> HandleAsync(
        Guid id,
        UpdateCategoryCommand command,
        ICommandHandler<UpdateCategoryCommand, bool> handler, 
        CancellationToken ct)
    {
        if (id != command.Id)
        {
            return Results.BadRequest("Category ID in route must match ID in request body.");
        }

        var success = await handler.Handle(command, ct);
        if (!success)
        {
            return Results.NotFound($"Category with ID '{id}' not found.");
        }
        return Results.Ok(); 
    }
}