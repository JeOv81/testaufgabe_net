using Application.Features.Products.Commands;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using ProductsApi.Filters;

namespace ProductsApi.Endpoints.Products;

public class CreateProductEndpoint : IEndpoint
{
    /// <inheritdoc />
    public void Map(IEndpointRouteBuilder app)
    {
        app.MapPost("/products", HandleAsync)
           .WithName("CreateProduct")
           .WithTags("Products")
           .AddEndpointFilter<ValidationFilter<CreateProductCommand>>()
           .Produces<Guid>(StatusCodes.Status201Created)
           .Produces(StatusCodes.Status400BadRequest)
           .Produces(StatusCodes.Status404NotFound);
    }
    public static async Task<IResult> HandleAsync(
        [FromBody] CreateProductCommand command,
        ICommandHandler<CreateProductCommand, Guid> handler,
        CancellationToken ct)
    {
        try
        {
            var productId = await handler.Handle(command, ct);
            return Results.Created($"/products/{productId}", productId);
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