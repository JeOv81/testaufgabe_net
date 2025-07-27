using Application.Features.Products.Queries;
using Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Application.Validations.Products;

public class GetProductByIdQueryValidator : AbstractValidator<GetProductByIdQuery>
{
    public GetProductByIdQueryValidator(IStringLocalizer<Validation> localizer)
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Product ID is required.")
            .Must(id => id != Guid.Empty).WithMessage("Product ID cannot be an empty GUID.");
    }
}