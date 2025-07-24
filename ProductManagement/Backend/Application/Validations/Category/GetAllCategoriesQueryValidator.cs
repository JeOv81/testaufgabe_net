using Application.Features.Categories.Commands;
using Application.Features.Categories.Queries;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validations.Category;

public class GetAllCategoriesQueryValidator : AbstractValidator<GetAllCategoriesQuery>
{
    public GetAllCategoriesQueryValidator()
    {
        RuleFor(x => x.SearchTerm)
           .MinimumLength(2).WithMessage("SearchTerm must be at least 2 characters long when provided.")
           .MaximumLength(50).WithMessage("SearchTerm must not exceed 50 characters.")
           .When(x => !string.IsNullOrWhiteSpace(x.SearchTerm));
    }
}