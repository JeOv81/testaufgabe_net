using Application.Features.Products.Commands;
using Application.Interfaces;
using Application.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using ProductsApi.Filters;

namespace ProductsApi.Endpoints.Products;

public class DeleteProductEndpoint : IEndpoint
{
    public void Map(IEndpointRouteBuilder app)
    {
        app.MapDelete("/products", HandleAsync)
           .RequireAuthorization("admin-policy")
           .WithName("DeleteProduct")
           .WithTags("Products")
           .AddEndpointFilter<ValidationFilter<DeleteProductCommand>>()
           .Produces(StatusCodes.Status204NoContent)
           .Produces(StatusCodes.Status404NotFound);
    }

    public static async Task<IResult> HandleAsync(
        [FromBody] DeleteProductCommand command,
        ICommandHandler<DeleteProductCommand, bool> handler,
        IStringLocalizer<MessageTemplate> localizer,
        CancellationToken ct)
    {
        var success = await handler.Handle(command, ct);
        if (!success)
        {
            return Results.NotFound(localizer["Template_Id_NotFound", "Product", command.Id].Value);
        }
        return Results.NoContent();
    }
}