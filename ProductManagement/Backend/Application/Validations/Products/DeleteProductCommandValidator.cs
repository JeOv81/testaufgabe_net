using Application.Features.Products.Commands;
using Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Application.Validations.Products;

public class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
{
    public DeleteProductCommandValidator(IStringLocalizer<Validation> localizer)
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Category ID must not be empty.");
    }
}