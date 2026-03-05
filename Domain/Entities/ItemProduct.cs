using Domain.Enums;

namespace Domain.Entities;

public class ItemProduct : BaseEntity
{
    public int ProductId { get; set; }
    public SizeProduct Size { get; set; }
    public Color Color { get; set; }
    public int Quantity { get; set; }
    public required List<string> Fhoto { get; set; }
    
    public Product? Product { get; set; }
}