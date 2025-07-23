using Application.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Products.Commands;
public class UpdateProductCommandHandler : ICommandHandler<UpdateProductCommand, bool>
{
    private readonly ProductsContext _context;

    public UpdateProductCommandHandler(ProductsContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _context.Products
                                    .Include(p => p.Categories)
                                    .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

        if (product == null)
        {
            return false; 
        }

        product.Name = request.Name;
        product.Price = request.Price;
        product.Description = request.Description;

        product.Categories.Clear();

        if (request.CategoryIds == null || !request.CategoryIds.Any())
        {
            throw new ArgumentException("At least one category must be assigned to the product.", nameof(request.CategoryIds));
        }

        foreach (var categoryId in request.CategoryIds)
        {
            var category = await _context.Categories.FindAsync(new object[] { categoryId }, cancellationToken);
            if (category == null)
            {
                throw new InvalidOperationException($"Category with ID '{categoryId}' not found.");
            }
            product.Categories.Add(category);
        }

        await _context.SaveChangesAsync(cancellationToken);

        return true; 
    }
}