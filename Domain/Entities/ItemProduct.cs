using System.ComponentModel.DataAnnotations;
using Domain.Enums;

namespace Domain.Entities;

public class ItemProduct : BaseEntity
{
    public int ProductId { get; set; }
    public SizeProduct Size { get; set; }
    public ColorProduct ColorProduct { get; set; }
    public int Quantity { get; set; }
    [Required] 
    public required List<string> Images { get; set; } = [];

    public Product? Product { get; set; }
}