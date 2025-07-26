using Application.Interfaces;

namespace Application.Features.Categories.Commands;
public class DeleteCategoryCommandHandler : ICommandHandler<DeleteCategoryCommand, bool>
{
    private readonly ICategoryRepository _repository;

    public DeleteCategoryCommandHandler(ICategoryRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {

        var category = await _repository.GetByIdAsync(request.Id, cancellationToken);

        if (category is null)
        {
            return false; 
        }

        if (category.Products.Any())
        {
            throw new InvalidOperationException($"Cannot delete category '{category.Name}' (ID: {category.Id}) because it is still assigned to {category.Products.Count} product(s). Remove product assignments first.");
        }

        await _repository.DeleteAsync(category.Id, cancellationToken);
        return true; 
    }
}