using Application.Features.Categories.Commands;
using Application.Interfaces;
using Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Application.Validations.Category;

public class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
{
    public UpdateCategoryCommandValidator(IStringLocalizer<Validation> localizer)
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage(localizer[Resources.Validation.Category_Id_Required]);

        

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(localizer["Category_Name_Required"])
            .Length(3, 50).WithMessage(localizer["Category_Name_Required"])
            .Matches(@"^[a-zA-Z0-9]+$").WithMessage(localizer["Category_Name_Alphanumeric"]);
    }
}