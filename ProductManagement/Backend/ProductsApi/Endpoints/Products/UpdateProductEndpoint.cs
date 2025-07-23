using Application.Features.Products.Commands;
using Application.Interfaces;

namespace ProductsApi.Endpoints.Products;

public class UpdateProductEndpoint : IEndpoint
{
    public void Map(IEndpointRouteBuilder app)
    {
        app.MapPut("/products/{id}", HandleAsync)
           .WithName("UpdateProduct")
           .WithTags("Products")
           .Produces(StatusCodes.Status200OK)
           .Produces(StatusCodes.Status400BadRequest)
           .Produces(StatusCodes.Status404NotFound);
    }

    public static async Task<IResult> HandleAsync(
        Guid id,
        UpdateProductCommand command,
        ICommandHandler<UpdateProductCommand, bool> handler,
        CancellationToken ct)
    {
        if (id != command.Id)
        {
            return Results.BadRequest("Product ID in route must match ID in request body.");
        }

        try
        {
            var success = await handler.Handle(command, ct);
            if (!success)
            {
                return Results.NotFound($"Product with ID '{id}' not found.");
            }
            return Results.Ok(); 
        }
        catch (InvalidOperationException ex)
        {
            return Results.NotFound(ex.Message);
        }
        catch (ArgumentException ex)
        {
            return Results.BadRequest(ex.Message);
        }
    }
}