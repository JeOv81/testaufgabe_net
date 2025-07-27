using Application.Features.Products.Commands;
using Application.Interfaces;
using Application.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using ProductsApi.Filters;

namespace ProductsApi.Endpoints.Products;

public class UpdateProductEndpoint : IEndpoint
{
    public void Map(IEndpointRouteBuilder app)
    {
        app.MapPut("/products", HandleAsync)
           .WithName("UpdateProduct")
           .WithTags("Products")
           .AddEndpointFilter<ValidationFilter<UpdateProductCommand>>()
           .Produces(StatusCodes.Status200OK)
           .Produces(StatusCodes.Status400BadRequest)
           .Produces(StatusCodes.Status404NotFound);
    }

    public static async Task<IResult> HandleAsync(
        [FromBody] UpdateProductCommand command,
        ICommandHandler<UpdateProductCommand, bool> handler,
        IStringLocalizer<MessageTemplate> localizer,
        CancellationToken ct)
    {
        try
        {
            var success = await handler.Handle(command, ct);
            if (!success)
            {
                return Results.NotFound(localizer["Template_Id_NotFound", "Product", command.Id].Value);
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