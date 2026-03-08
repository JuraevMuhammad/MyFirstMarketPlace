using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Category;

public class CreatedCategory
{
    [Required]
    public required string Name { get; set; }
}