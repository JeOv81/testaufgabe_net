using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace Infrastructure.Repositories;
public class ProductRepository : IProductRepository
{
    private readonly ProductsContext _productsContext;

    public ProductRepository(ProductsContext productsContext)
    {
        _productsContext = productsContext;
    }

    public async Task<Guid> CreateAsync(Product product, ICollection<Guid> categoryIds, CancellationToken cancellationToken = default)
    {
        var categories = await _productsContext.Categories
            .Where(c => categoryIds.Contains(c.Id))
            .ToListAsync(cancellationToken);

        product.Categories = categories;

        _productsContext.Products.Add(product);
        await _productsContext.SaveChangesAsync(cancellationToken);
        return product.Id;
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var product = await _productsContext.Products.FindAsync(new object[] { id }, cancellationToken);
        if (product != null)
        {
            _productsContext.Products.Remove(product);
            await _productsContext.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<IReadOnlyList<Product>> GetAllAsync(
    string? searchTerm = null,
    int pageNumber = 1,
    int pageSize = 10,
    string? orderBy = null,
    bool? ascending = true,
    CancellationToken cancellationToken = default)
    {
        var query = _productsContext.Products
            .Include(p => p.Categories)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(p =>
                p.Name.Contains(searchTerm) ||
                (p.Description != null && p.Description.Contains(searchTerm)));
        }

        var orderProperty = string.IsNullOrWhiteSpace(orderBy) ? "Name" : orderBy;

        if (typeof(Product).GetProperty(orderProperty) is null)
        {
            orderProperty = "Name";
        }

        var orderString = ascending == true ? orderProperty : $"{orderProperty} descending";
        query = query.OrderBy(orderString); 

        if (pageNumber > 0 && pageSize > 0)
        {
            query = query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);
        }

        return await query.ToListAsync(cancellationToken);
    }

    public async Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _productsContext.Products.FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task<Product?> GetByIdWithCategoriesAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _productsContext.Products
            .Include(p => p.Categories)
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public async Task UpdateAsync(Product product, ICollection<Guid> categoryIds, CancellationToken cancellationToken = default)
    {
        var existingProduct = await _productsContext.Products
            .Include(p => p.Categories)
            .FirstOrDefaultAsync(p => p.Id == product.Id, cancellationToken);

        if (existingProduct == null)
            throw new InvalidOperationException("Product not found.");

        existingProduct.Name = product.Name;
        existingProduct.Price = product.Price;
        existingProduct.Description = product.Description;

        var categories = await _productsContext.Categories
            .Where(c => categoryIds.Contains(c.Id))
            .ToListAsync(cancellationToken);

        existingProduct.Categories = categories;

        _productsContext.Products.Update(existingProduct);
        await _productsContext.SaveChangesAsync(cancellationToken);
    }
}