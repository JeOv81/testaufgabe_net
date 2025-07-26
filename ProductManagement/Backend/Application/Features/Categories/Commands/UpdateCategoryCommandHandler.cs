using Application.Interfaces;

namespace Application.Features.Categories.Commands;
public class UpdateCategoryCommandHandler : ICommandHandler<UpdateCategoryCommand, bool>
{
    private readonly ICategoryRepository _repository;

    public UpdateCategoryCommandHandler(ICategoryRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _repository.GetByIdAsync(request.Id, cancellationToken);

        if (category is null)
        {
            return false;
        }

        category.Name = request.Name;

        await _repository.UpdateAsync(category, cancellationToken);
        return true;
    }
}