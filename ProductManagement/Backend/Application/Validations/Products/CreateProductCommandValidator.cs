using Application.Features.Products.Commands;
using FluentValidation;

namespace Application.Validations.Products;

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Product name is required.")
            .Length(3, 100).WithMessage("Product name must be between 3 and 100 characters.")
            .Matches(@"^[a-zA-Z0-9]+$").WithMessage("Product name can only contain alphanumeric characters (no spaces or special chars).");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Price must be greater than zero.");

        RuleFor(x => x.CategoryIds)
            .NotEmpty().WithMessage("At least one category must be assigned.")
            .Must(ids => ids != null && ids.All(id => id != Guid.Empty)).WithMessage("Category IDs cannot be empty GUIDs.");
    }
}