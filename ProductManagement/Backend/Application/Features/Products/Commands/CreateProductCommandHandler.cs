using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Persistence;

namespace Application.Features.Products.Commands;
public class CreateProductCommandHandler : ICommandHandler<CreateProductCommand, Guid>
{
    private readonly ProductsContext _context; // Oder ein IProductRepository

    public CreateProductCommandHandler(ProductsContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var product = new Product
        {
            Name = request.Name,
            Price = request.Price,
            Description = request.Description
        };

        foreach (var categoryId in request.CategoryIds)
        {
            var category = await _context.Categories.FindAsync(categoryId);
            if (category == null)
            {
                throw new Exception($"Category with ID {categoryId} not found.");
            }
            product.Categories.Add(category);
        }

        await _context.Products.AddAsync(product, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return product.Id; 
    }
}