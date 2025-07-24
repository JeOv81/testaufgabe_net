using Application.Features.Categories.Commands;
using Application.Features.Categories.Queries;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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