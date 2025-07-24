using Application.Features.Products.Commands;
using FluentValidation;

namespace Application.Validations.Products;

public class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
{
    public DeleteProductCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Category ID must not be empty.");
    }
}