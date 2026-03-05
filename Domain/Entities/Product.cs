using System.ComponentModel.DataAnnotations;
using Domain.Enums;

namespace Domain.Entities;

public class Product : BaseEntity
{
    public int CategoryId { get; set; }
    public int UserId { get; set; }
    [Required]
    public required string Name { get; set; }
    [Required]
    [StringLength(150, MinimumLength = 20)]
    public required string Description { get; set; }
    [Required]
    public required decimal Price { get; set; }
    
    public List<ItemProduct>? ItemProducts { get; set; }
    public List<Order>? Orders { get; set; }
    public Category? Category { get; set; }
    public User? User { get; set; }
}