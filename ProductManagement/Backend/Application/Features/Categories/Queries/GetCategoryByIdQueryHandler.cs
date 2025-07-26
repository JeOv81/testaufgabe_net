using Application.DTOs;
using Application.Interfaces;

namespace Application.Features.Categories.Queries;
public class GetCategoryByIdQueryHandler : IQueryHandler<GetCategoryByIdQuery, CategoryDto?>
{
    private readonly ICategoryRepository _repository;

    public GetCategoryByIdQueryHandler(ICategoryRepository repository)
    {
        _repository = repository;
    }

    public async Task<CategoryDto?> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        var category = await _repository.GetByIdAsync(request.Id, cancellationToken);

        if (category is null)
        {
            return null;
        }

        return new CategoryDto(
            category.Id,
            category.Name
        );
    }
}