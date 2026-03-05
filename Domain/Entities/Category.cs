using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Category : BaseEntity
{
    [Required]
    public required string Name { get; set; }
    
    public List<Product>? Products { get; set; }
}