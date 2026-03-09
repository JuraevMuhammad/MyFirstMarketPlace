using Domain.Enums;

namespace Domain.Entities;

public class ItemProduct : BaseEntity
{
    public int ProductId { get; set; }
    public SizeProduct Size { get; set; }
    public ColorProduct ColorProduct { get; set; }
    public int Quantity { get; set; }
    
    public Product? Product { get; set; }
}