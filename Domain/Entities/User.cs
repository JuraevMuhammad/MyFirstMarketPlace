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
    public Roles Role { get; set; }
}