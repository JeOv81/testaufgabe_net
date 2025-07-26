using Application.DTOs;
using Application.Interfaces;

namespace Application.Features.Products.Queries;
public class GetAllProductsQueryHandler : IQueryHandler<GetAllProductsQuery, ICollection<ProductDto>>
{
    private readonly IProductRepository _repository;

    public GetAllProductsQueryHandler(IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<ICollection<ProductDto>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
    {
        var products = await _repository.GetAllAsync(request.SearchTerm, request.PageNumber, request.PageSize, request.OrderBy, request.Ascending, cancellationToken);

        return [.. products.Select(product => new ProductDto(
            product.Id,
            product.Name,
            product.Price,
            product.Description,
            [.. product.Categories.Select(c => new CategoryDto(c.Id, c.Name))]
        ))];
    }
}
