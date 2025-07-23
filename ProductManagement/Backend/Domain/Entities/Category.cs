using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities;
public class Category : IGuidEntity
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public virtual ICollection<Product> Products { get; set; } = [];
}
