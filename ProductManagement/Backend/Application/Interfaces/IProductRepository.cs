using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces;
public interface IProductRepository
{
    Task<Guid> CreateAsync(Product product, ICollection<Guid> categoryIds, CancellationToken cancellationToken = default);
    Task UpdateAsync(Product product, ICollection<Guid> categoryIds, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Product>> GetAllAsync(
        string? searchTerm = null,
        int pageNumber = 1,
        int pageSize = 10,
        string? orderBy = null,
        bool? ascending = true,
        CancellationToken cancellationToken = default);

    Task<Product?> GetByIdWithCategoriesAsync(Guid id, CancellationToken cancellationToken = default);
}
