using System.ComponentModel.DataAnnotations;
using Domain.Entities;

namespace Application.DTOs.Product;

public class CreatedProduct
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
    
    public List<CreatedProduct>? ItemProducts { get; set; }
}