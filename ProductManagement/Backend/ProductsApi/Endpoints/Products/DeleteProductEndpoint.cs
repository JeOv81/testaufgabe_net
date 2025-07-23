using Application.Features.Products.Commands;
using Application.Interfaces;

namespace ProductsApi.Endpoints.Products;

public class DeleteProductEndpoint : IEndpoint
{
    public void Map(IEndpointRouteBuilder app)
    {
        app.MapDelete("/products/{id}", HandleAsync)
           .WithName("DeleteProduct")
           .WithTags("Products")
           .Produces(StatusCodes.Status204NoContent) 
           .Produces(StatusCodes.Status404NotFound);
    }

    public static async Task<IResult> HandleAsync(
        Guid id,
        ICommandHandler<DeleteProductCommand, bool> handler,
        CancellationToken ct)
    {
        var success = await handler.Handle(new DeleteProductCommand(id), ct);
        if (!success)
        {
            return Results.NotFound($"Product with ID '{id}' not found.");
        }
        return Results.NoContent(); 
    }
}