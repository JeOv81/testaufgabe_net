using Application.Features.Categories.Commands;
using Application.Interfaces;
using Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Application.Validations.Category;
public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidator(IStringLocalizer<Validation> localizer)
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(localizer["Category_Name_Required"])
            .Length(3, 50).WithMessage(localizer["Category_Name_Length"])
            .Matches(@"^[a-zA-Z0-9]+$").WithMessage(localizer["Category_Name_Alphanumeric"]);
    }
}