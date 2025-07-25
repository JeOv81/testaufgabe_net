using Application.Features.Categories.Commands;
using FluentValidation;

namespace Application.Validations.Category;

public class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
{
    public UpdateCategoryCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Category ID is required for update.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Category name is required.")
            .Length(3, 50).WithMessage("Category name must be between 3 and 50 characters.")
            .Matches(@"^[a-zA-Z0-9]+$").WithMessage("Category name can only contain alphanumeric characters (no spaces or special characters).");
    }
}