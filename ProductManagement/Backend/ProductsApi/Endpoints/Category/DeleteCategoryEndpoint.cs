using Application.Features.Categories.Commands;
using Application.Interfaces;
using Application.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using ProductsApi.Filters;

namespace ProductsApi.Endpoints.Category;

public class DeleteCategoryEndpoint : IEndpoint
{
    public void Map(IEndpointRouteBuilder app)
    {
        app.MapDelete("/categories", HandleAsync)
           .RequireAuthorization("admin-policy")
           .WithName("DeleteCategory")
           .WithTags("Categories")
           .AddEndpointFilter<ValidationFilter<DeleteCategoryCommand>>()
           .Produces(StatusCodes.Status204NoContent)
           .Produces(StatusCodes.Status404NotFound)
           .Produces(StatusCodes.Status400BadRequest); 
    }

    public static async Task<IResult> HandleAsync(
        [FromBody] DeleteCategoryCommand command,
        ICommandHandler<DeleteCategoryCommand, bool> handler,
        IStringLocalizer<MessageTemplate> localizer,
        CancellationToken ct)
    {
        try
        {
            var success = await handler.Handle(command, ct);
            if (!success)
            {
                return Results.NotFound(localizer["Template_Id_NotFound", "Category", command.Id].Value);
            }
            return Results.NoContent();
        }
        catch (InvalidOperationException ex) 
        {
            return Results.BadRequest(ex.Message);
        }
    }
}