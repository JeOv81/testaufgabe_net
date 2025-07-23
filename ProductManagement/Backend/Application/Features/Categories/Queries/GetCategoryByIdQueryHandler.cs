using Application.DTOs;
using Application.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Categories.Queries;
public class GetCategoryByIdQueryHandler : IQueryHandler<GetCategoryByIdQuery, CategoryDto?>
{
    private readonly ProductsContext _context;

    public GetCategoryByIdQueryHandler(ProductsContext context)
    {
        _context = context;
    }

    public async Task<CategoryDto?> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        var category = await _context.Categories
                                     .AsNoTracking() 
                                     .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        if (category == null)
        {
            return null;
        }

        return new CategoryDto(
            category.Id,
            category.Name
        );
    }
}