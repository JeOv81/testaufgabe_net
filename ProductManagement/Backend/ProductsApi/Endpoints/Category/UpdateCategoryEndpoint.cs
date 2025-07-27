using Application.Features.Categories.Commands;
using Application.Interfaces;
using Application.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
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
        IStringLocalizer<MessageTemplate> localizer,
        CancellationToken ct)
    {
        var success = await handler.Handle(command, ct);
        if (!success)
        {
            return Results.NotFound(localizer["Template_Id_NotFound", "Category", command.Id].Value);
        }
        return Results.Ok(); 
    }
}