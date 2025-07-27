using Application.Features.Categories.Queries;
using Application.Interfaces;
using Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Application.Validations.Category;

public class GetAllCategoriesQueryValidator : AbstractValidator<GetAllCategoriesQuery>
{
    public GetAllCategoriesQueryValidator(IStringLocalizer<Validation> localizer)
    {
        RuleFor(x => x.SearchTerm)
           .MinimumLength(2).WithMessage(localizer["Category_SearchTerm_Minimum"])
           .MaximumLength(50).WithMessage(localizer["Category_SearchTerm_Maximum"])
           .When(x => !string.IsNullOrWhiteSpace(x.SearchTerm));
    }
}