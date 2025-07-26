using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces;
public interface ICategoryRepository
{
    Task<Guid> CreateAsync(Category category, CancellationToken cancellationToken = default);
    Task UpdateAsync(Category category, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Category?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Category>> GetAllAsync(string? searchTerm = null, CancellationToken cancellationToken = default);

    Task<bool> AreValidCategoryIdsAsync(ICollection<Guid> categoryIds, CancellationToken cancellationToken = default);
}
