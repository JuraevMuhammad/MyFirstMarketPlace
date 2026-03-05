using System.ComponentModel.DataAnnotations;
using Domain.Enums;

namespace Application.DTOs.User;

public class CreatedUser
{
    [Required]
    public required string Username { get; set; }
    [Required]
    public required string Password { get; set; }
    [Required]
    public required string Email { get; set; }
    public Roles Role { get; set; } = Roles.Customer;
}