using Domain.Enums;

namespace Application.DTOs.ItemProduct;

public class GetItemProduct
{
    public SizeProduct? Size { get; set; }
    public ColorProduct? Color { get; set; }
    public int Quantity { get; set; }
    public List<string> Images { get; set; } = [];
}