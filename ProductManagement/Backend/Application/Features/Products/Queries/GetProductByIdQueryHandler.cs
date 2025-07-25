using Application.DTOs;
using Application.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Products.Queries;
public class GetProductByIdQueryHandler : IQueryHandler<GetProductByIdQuery, ProductDto?>
{
    private readonly ProductsContext _context;

    public GetProductByIdQueryHandler(ProductsContext context)
    {
        _context = context;
    }

    public async Task<ProductDto?> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await _context.Products
                                    .Include(p => p.Categories) 
                                    .AsNoTracking() 
                                    .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

        if (product == null)
        {
            return null;
        }

        return new ProductDto(
            product.Id,
            product.Name,
            product.Price,
            product.Description,
            product.Categories.Select(c => new CategoryDto(c.Id, c.Name)).ToList() 
        );
    }
}