using Application.Features.Categories.Queries;
using FluentValidation;

namespace Application.Validations.Category;

public class GetCategoriesByIdQueryValidator : AbstractValidator<GetCategoryByIdQuery>
{
    public GetCategoriesByIdQueryValidator()
    {
        RuleFor(x => x.Id)
           .NotEmpty()
           .WithMessage("Category ID must not be empty.");
    }
}