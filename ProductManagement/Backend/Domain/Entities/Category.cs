using Domain.Interfaces;

namespace Domain.Entities;
public class Category : IGuidEntity
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public virtual ICollection<Product> Products { get; set; } = [];
}
