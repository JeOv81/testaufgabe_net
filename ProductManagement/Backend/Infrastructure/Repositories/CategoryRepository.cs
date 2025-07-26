using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;
public class CategoryRepository : ICategoryRepository
{
    private readonly ProductsContext _productsContext;

    public CategoryRepository(ProductsContext productsContext)
    {
        _productsContext = productsContext;
    }

    public async Task<bool> AreValidCategoryIdsAsync(ICollection<Guid> categoryIds, CancellationToken cancellationToken = default)
    {
        if (categoryIds == null || !categoryIds.Any())
            return false;

        var existingIds = await _productsContext.Categories
            .Where(c => categoryIds.Contains(c.Id))
            .Select(c => c.Id)
            .ToListAsync(cancellationToken);

        return existingIds.Count == categoryIds.Count;
    }

    public async Task<Guid> CreateAsync(Category category, CancellationToken cancellationToken = default)
    {
        _productsContext.Categories.Add(category);
        await _productsContext.SaveChangesAsync(cancellationToken);
        return category.Id;
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var category = await _productsContext.Categories.FindAsync(new object[] { id }, cancellationToken);
        if (category != null)
        {
            _productsContext.Categories.Remove(category);
            await _productsContext.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<IReadOnlyList<Category>> GetAllAsync(string? searchTerm = null, CancellationToken cancellationToken = default)
    {
        var query = _productsContext.Categories.AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(c => c.Name.Contains(searchTerm));
        }

        return await query.ToListAsync(cancellationToken);
    }

    public async Task<Category?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _productsContext.Categories.FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task UpdateAsync(Category category, CancellationToken cancellationToken = default)
    {
        _productsContext.Categories.Update(category);
        await _productsContext.SaveChangesAsync(cancellationToken);
    }
}