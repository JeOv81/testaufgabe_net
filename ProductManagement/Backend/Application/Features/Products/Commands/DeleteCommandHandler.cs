using Application.Interfaces;
using Infrastructure.Persistence;

namespace Application.Features.Products.Commands;
public class DeleteProductCommandHandler : ICommandHandler<DeleteProductCommand, bool>
{
    private readonly ProductsContext _context;

    public DeleteProductCommandHandler(ProductsContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _context.Products.FindAsync([request.Id], cancellationToken);

        if (product == null)
        {
            return false; 
        }

        _context.Products.Remove(product);
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}