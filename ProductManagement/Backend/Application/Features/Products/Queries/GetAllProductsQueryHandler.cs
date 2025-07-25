using Application.DTOs;
using Application.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace Application.Features.Products.Queries;
public class GetAllProductsQueryHandler : IQueryHandler<GetAllProductsQuery, ICollection<ProductDto>>
{
    private readonly ProductsContext _context;

    public GetAllProductsQueryHandler(ProductsContext context)
    {
        _context = context;
    }

    public async Task<ICollection<ProductDto>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
    {
        IQueryable<Domain.Entities.Product> query = _context.Products
                                                              .Include(p => p.Categories) 
                                                              .AsNoTracking();

        // Filterung nach Suchbegriff
        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            query = query.Where(p => p.Name.Contains(request.SearchTerm) ||
                                     (p.Description != null && p.Description.Contains(request.SearchTerm)));
        }

        if (string.IsNullOrWhiteSpace(request.OrderBy))
        {
            query = query.OrderBy(p => p.Name);
        }
        else
        {
            var type = typeof(Domain.Entities.Product);
            if (type.GetProperty(request.OrderBy) is not null)
            {
                if(request.Ascending is true)
                { 
                    query = query.OrderBy(request.OrderBy)
                                 .Reverse();
                }
                else
                {
                    query = query.OrderBy(request.OrderBy);
                }
            }
            else
            {
                query = query.OrderBy(p => p.Name);
            }
        }

        // Pagination
        if (request.PageSize > 0 && request.PageNumber > 0)
        {
            query = query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize);
        }

        var products = await query.ToListAsync(cancellationToken);

        return [.. products.Select(product => new ProductDto(
            product.Id,
            product.Name,
            product.Price,
            product.Description,
            [.. product.Categories.Select(c => new CategoryDto(c.Id, c.Name))]
        ))];
    }
}
