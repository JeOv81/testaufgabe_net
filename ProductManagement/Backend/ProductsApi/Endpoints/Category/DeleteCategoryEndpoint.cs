using Application.Features.Categories.Commands;
using Application.Interfaces;

namespace ProductsApi.Endpoints.Category;

public class DeleteCategoryEndpoint : IEndpoint
{
    public void Map(IEndpointRouteBuilder app)
    {
        app.MapDelete("/categories/{id}", HandleAsync)
           .WithName("DeleteCategory")
           .WithTags("Categories")
           .Produces(StatusCodes.Status204NoContent)
           .Produces(StatusCodes.Status404NotFound)
           .Produces(StatusCodes.Status400BadRequest); 
    }

    public static async Task<IResult> HandleAsync(
        Guid id,
        ICommandHandler<DeleteCategoryCommand, bool> handler,
        CancellationToken ct)
    {
        try
        {
            var success = await handler.Handle(new DeleteCategoryCommand(id), ct);
            if (!success)
            {
                return Results.NotFound($"Category with ID '{id}' not found.");
            }
            return Results.NoContent();
        }
        catch (InvalidOperationException ex) 
        {
            return Results.BadRequest(ex.Message);
        }
    }
}