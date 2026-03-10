using Domain.Enums;

namespace Application.DTOs.ItemProduct;

public class UpdateItemProduct
{
    public SizeProduct? Size { get; set; }
    public ColorProduct? Color { get; set; }
    public int? Quantity { get; set; }
}