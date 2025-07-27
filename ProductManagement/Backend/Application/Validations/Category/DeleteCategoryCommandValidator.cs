using Application.Features.Categories.Commands;
using Application.Interfaces;
using Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Application.Validations.Category;
public class DeleteCategoryCommandValidator : AbstractValidator<DeleteCategoryCommand>
{
    public DeleteCategoryCommandValidator(IStringLocalizer<Validation> localizer)
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage(localizer["Category_Id_Required"]);
    }
}