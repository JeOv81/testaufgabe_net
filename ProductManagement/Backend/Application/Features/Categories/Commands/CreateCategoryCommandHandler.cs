using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Persistence;

namespace Application.Features.Categories.Commands;
public class CreateCategoryCommandHandler : ICommandHandler<CreateCategoryCommand, Guid>
{
    private readonly ProductsContext _context;

    public CreateCategoryCommandHandler(ProductsContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = new Category
        {
            Name = request.Name
        };

        await _context.Categories.AddAsync(category, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return category.Id;
    }
}