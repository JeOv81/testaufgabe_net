using Application.Interfaces;
using Domain.Entities;

namespace Application.Features.Categories.Commands;
public class CreateCategoryCommandHandler : ICommandHandler<CreateCategoryCommand, Guid>
{
    private readonly ICategoryRepository _repository;

    public CreateCategoryCommandHandler(ICategoryRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = new Category
        {
            Name = request.Name
        };

        await _repository.CreateAsync(category, cancellationToken);
        return category.Id;
    }
}