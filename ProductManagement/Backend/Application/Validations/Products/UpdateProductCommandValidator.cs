using Application.Features.Products.Commands;
using Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Application.Validations.Products;
public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator(IStringLocalizer<Validation> localizer)
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Product ID is required for update.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Product name is required.")
            .Length(3, 100).WithMessage("Product name must be between 3 and 100 characters.")
            .Matches(@"^[a-zA-Z0-9]+$").WithMessage("Product name can only contain alphanumeric characters (no spaces or special characters).");

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Description cannot exceed 500 characters.");

        RuleFor(x => x.CategoryIds)
            .NotEmpty().WithMessage("At least one category must be assigned.")
            .Must(ids => ids != null && ids.All(id => id != Guid.Empty)).WithMessage("Category IDs cannot be empty GUIDs.");
    }
}