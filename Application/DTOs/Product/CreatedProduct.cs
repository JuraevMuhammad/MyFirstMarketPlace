using System.ComponentModel.DataAnnotations;
using Application.DTOs.ItemProduct;
using Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Application.DTOs.Product;

public class CreatedProduct
{
    public int CategoryId { get; set; }
    [Required]
    public required string Name { get; set; }
    [Required]
    [StringLength(150, MinimumLength = 20)]
    public required string Description { get; set; }
    [Required]
    public required decimal Price { get; set; }
    [Required]
    public required List<IFormFile> Images { get; set; }
}