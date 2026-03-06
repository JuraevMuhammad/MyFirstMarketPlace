using System.ComponentModel.DataAnnotations;
using Domain.Enums;

namespace Domain.Entities;

public class User : BaseEntity
{
    [Required]
    public required string Username { get; set; }
    [Required]
    public required string HashPassword { get; set; }
    [Required]
    public required string Email { get; set; }
    public Roles Role { get; set; } = Roles.Customer;
    
    public Finance? Finance { get; set; }
    public List<Product>? Products { get; set; }
    public List<Order>? Orders { get; set; }
}