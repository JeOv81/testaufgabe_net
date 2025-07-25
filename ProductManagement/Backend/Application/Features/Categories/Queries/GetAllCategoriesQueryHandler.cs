using Application.DTOs;
using Application.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Categories.Queries;

public class GetAllCategoriesQueryHandler : IQueryHandler<GetAllCategoriesQuery, ICollection<CategoryDto>>
{
    private readonly ProductsContext _context;

    public GetAllCategoriesQueryHandler(ProductsContext context)
    {
        _context = context;
    }

    public async Task<ICollection<CategoryDto>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
    {
        IQueryable<Domain.Entities.Category> query = _context.Categories
                                                              .AsNoTracking();

        // Filterung nach Suchbegriff
        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            query = query.Where(c => c.Name.Contains(request.SearchTerm));
        }

        var categories = await query.ToListAsync(cancellationToken);

        // Mapping von Entitäten zu DTOs
        return [.. categories.Select(category => new CategoryDto(
            category.Id,
            category.Name
        ))];
    }
}