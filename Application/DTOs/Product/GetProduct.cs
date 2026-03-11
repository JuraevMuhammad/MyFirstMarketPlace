using Application.DTOs.ItemProduct;

namespace Application.DTOs.Product;

public class GetProduct
{
    public int Id { get; set; }
    public int CategoryId { get; set; }
    public int UserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public List<string> Images { get; set; } = [];
    
    public List<GetItemProduct> ItemProducts { get; set; } = [];
}