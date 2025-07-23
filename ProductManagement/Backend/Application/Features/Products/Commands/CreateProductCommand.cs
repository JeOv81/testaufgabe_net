namespace Application.Features.Products.Commands;
public record CreateProductCommand(string Name, decimal Price, string? Description, ICollection<Guid> CategoryIds);
