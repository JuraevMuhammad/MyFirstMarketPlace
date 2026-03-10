namespace Application.Filter;

public class ProductFilter : BaseFilter
{
    public string? Name { get; set; }
    public decimal? MaxPrice { get; set; }
    public decimal? MinPrice { get; set; }
    public int? CategoryId { get; set; }
}