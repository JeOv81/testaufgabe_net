using Application.Interfaces;

namespace Application.Features.Products.Commands;
public class DeleteProductCommandHandler : ICommandHandler<DeleteProductCommand, bool>
{
    private readonly IProductRepository _repository;

    public DeleteProductCommandHandler(IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _repository.GetByIdAsync(request.Id, cancellationToken);

        if (product is null)
        {
            return false; 
        }

        await _repository.DeleteAsync(product.Id, cancellationToken);
        return true;
    }
}