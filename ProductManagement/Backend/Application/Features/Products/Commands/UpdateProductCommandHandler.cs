using Application.Interfaces;

namespace Application.Features.Products.Commands;
public class UpdateProductCommandHandler : ICommandHandler<UpdateProductCommand, bool>
{
    private readonly IProductRepository _repository;
    private readonly ICategoryRepository _categoryRepository;

    public UpdateProductCommandHandler(IProductRepository repository, ICategoryRepository categoryRepository)
    {
        _repository = repository;
        _categoryRepository = categoryRepository;
    }

    public async Task<bool> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _repository.GetByIdWithCategoriesAsync(request.Id, cancellationToken);
        if (product is null)
        {
            return false;
        }

        if (request.CategoryIds == null || request.CategoryIds.Count == 0)
        {
            throw new ArgumentException("At least one category must be assigned to the product.", nameof(request.CategoryIds));
        }

        var areValid = await _categoryRepository.AreValidCategoryIdsAsync(request.CategoryIds, cancellationToken);
        if (!areValid)
        {
            throw new InvalidOperationException("One or more categories do not exist.");
        }

        product.Name = request.Name;
        product.Price = request.Price;
        product.Description = request.Description;

        await _repository.UpdateAsync(product, request.CategoryIds, cancellationToken);
        return true;
    }
}