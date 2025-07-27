using Application.Features.Products.Commands;
using Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Application.Validations.Products;

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator(IStringLocalizer<Validation> localizer)
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(localizer["Product_Id_Required"])
            .Length(3, 100).WithMessage(localizer["Product_Name_Length"])
            .Matches(@"^[a-zA-Z0-9]+$").WithMessage(localizer["Product_Name_Alphanumeric"]);

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage(localizer["Product_Price_GreaterThan"]);

        RuleFor(x => x.CategoryIds)
            .NotEmpty().WithMessage(localizer["Product_Category_Missing"])
            .Must(ids => ids != null && ids.All(id => id != Guid.Empty)).WithMessage(localizer["Product_Category_Empty"]);
    }
}