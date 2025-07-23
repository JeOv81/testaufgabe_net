using Application.Interfaces;
using Infrastructure.Persistence;

namespace Application.Features.Categories.Commands;
public class UpdateCategoryCommandHandler : ICommandHandler<UpdateCategoryCommand, bool>
{
    private readonly ProductsContext _context;

    public UpdateCategoryCommandHandler(ProductsContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _context.Categories.FindAsync(new object[] { request.Id }, cancellationToken);

        if (category == null)
        {
            return false;
        }

        category.Name = request.Name;

        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}