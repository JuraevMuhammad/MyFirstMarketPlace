using System.ComponentModel.DataAnnotations;
using Domain.Enums;

namespace Domain.Entities;

public class Order : BaseEntity
{
    public required SizeProduct SizeProduct { get; set; }
    public required Color Color { get; set; }
    [Required]
    [Phone]
    public required string PhoneNumber { get; set; }
    [Required]
    public required string Name { get; set; }
    public decimal Sum { get; set; }
    
    public int UserId { get; set; }
    public User? User { get; set; }
    public int ProductId { get; set; }
    public Product? Product { get; set; }
}