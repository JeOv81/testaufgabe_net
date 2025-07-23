namespace Application.Features.Products.Commands;
public record UpdateProductCommand(Guid Id, string Name, decimal Price, string? Description, ICollection<Guid> CategoryIds);
