using Application.Features.Categories.Queries;
using Application.Interfaces;
using Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Application.Validations.Category;

public class GetCategoriesByIdQueryValidator : AbstractValidator<GetCategoryByIdQuery>
{
    public GetCategoriesByIdQueryValidator(IStringLocalizer<Validation> localizer)
    {
        RuleFor(x => x.Id)
           .NotEmpty()
           .WithMessage(localizer["Category_Id_Required"]);
    }
}