using Application.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Categories.Commands;
public class DeleteCategoryCommandHandler : ICommandHandler<DeleteCategoryCommand, bool>
{
    private readonly ProductsContext _context;

    public DeleteCategoryCommandHandler(ProductsContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {

        var category = await _context.Categories
                                     .Include(c => c.Products)
                                     .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        if (category == null)
        {
            return false; 
        }

        if (category.Products.Any())
        {
            throw new InvalidOperationException($"Cannot delete category '{category.Name}' (ID: {category.Id}) because it is still assigned to {category.Products.Count} product(s). Remove product assignments first.");
        }

        _context.Categories.Remove(category);
        await _context.SaveChangesAsync(cancellationToken);

        return true; 
    }
}