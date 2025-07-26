using Application.Interfaces;
using Domain.Entities;

namespace Application.Features.Products.Commands;
public class CreateProductCommandHandler : ICommandHandler<CreateProductCommand, Guid>
{
    private readonly IProductRepository _repository;
    private readonly ICategoryRepository _categoryRepository;

    public CreateProductCommandHandler(IProductRepository repository, ICategoryRepository categoryRepository)
    {
        _repository = repository;
        _categoryRepository = categoryRepository;
    }

    public async Task<Guid> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        if (request.CategoryIds == null || request.CategoryIds.Count == 0)
        {
            throw new ArgumentException("At least one category must be assigned to the product.", nameof(request.CategoryIds));
        }

        var areValid = await _categoryRepository.AreValidCategoryIdsAsync(request.CategoryIds, cancellationToken);
        if (!areValid)
        {
            throw new InvalidOperationException("One or more categories do not exist.");
        }

        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Price = request.Price,
            Description = request.Description,
            Categories = new List<Category>()
        };

        foreach (var categoryId in request.CategoryIds)
        {
            var category = await _categoryRepository.GetByIdAsync(categoryId, cancellationToken);
            if (category != null)
            {
                product.Categories.Add(category);
            }
        }

        await _repository.CreateAsync(product, request.CategoryIds, cancellationToken);
        return product.Id;
    }
}