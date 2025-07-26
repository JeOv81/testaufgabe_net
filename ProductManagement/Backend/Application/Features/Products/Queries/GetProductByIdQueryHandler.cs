using Application.DTOs;
using Application.Interfaces;

namespace Application.Features.Products.Queries;
public class GetProductByIdQueryHandler : IQueryHandler<GetProductByIdQuery, ProductDto?>
{
    private readonly IProductRepository _repository;

    public GetProductByIdQueryHandler(IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<ProductDto?> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await _repository.GetByIdAsync(request.Id, cancellationToken);

        return product is null
            ? null
            : new ProductDto(
            product.Id,
            product.Name,
            product.Price,
            product.Description,
            [.. product.Categories.Select(c => new CategoryDto(c.Id, c.Name))]
        );
    }
}