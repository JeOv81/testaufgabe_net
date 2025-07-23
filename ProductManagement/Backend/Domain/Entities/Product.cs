using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities;
public class Product : IGuidEntity
{
    public Guid Id { get; set; }
    public required string Name { get; set; } 
    public decimal Price { get; set; } 
    public string? Description { get; set; }

    public virtual ICollection<Category> Categories { get; set; } = [];
}
