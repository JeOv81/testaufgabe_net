using Application.Features.Products.Queries;
using FluentValidation;

namespace Application.Validations.Products;
public class GetAllProductsQueryValidator : AbstractValidator<GetAllProductsQuery>
{
    public GetAllProductsQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1)
            .WithMessage("PageNumber must be at least 1.");

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(100) // Oder einen anderen Maximalwert, der für deine API sinnvoll ist
            .WithMessage("PageSize must be greater than 0 and less than or equal to 100.");

        RuleFor(x => x.SearchTerm)
            .MinimumLength(2).WithMessage("SearchTerm must be at least 2 characters long when provided.")
            .MaximumLength(50).WithMessage("SearchTerm must not exceed 50 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.SearchTerm));
    }
}