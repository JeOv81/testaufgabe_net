using Application.DTOs;
using Application.Interfaces;

namespace Application.Features.Categories.Queries;

public class GetAllCategoriesQueryHandler : IQueryHandler<GetAllCategoriesQuery, ICollection<CategoryDto>>
{
    private readonly ICategoryRepository _repository;

    public GetAllCategoriesQueryHandler(ICategoryRepository repository)
    {
        _repository = repository;
    }

    public async Task<ICollection<CategoryDto>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
    {
        var categories = await _repository.GetAllAsync(request.SearchTerm, cancellationToken);

        return [.. categories.Select(category => new CategoryDto(
            category.Id,
            category.Name
        ))];
    }
}