using Domain.Interfaces;

namespace Domain.Entities;
public class Product : IGuidEntity
{
    public Guid Id { get; set; }
    public required string Name { get; set; } 
    public decimal Price { get; set; } 
    public string? Description { get; set; }

    public virtual ICollection<Category> Categories { get; set; } = [];
}
