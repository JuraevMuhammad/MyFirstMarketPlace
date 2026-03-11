namespace Application.Filter;

public class FinanceFilter : BaseFilter
{
    public DateTime? Start { get; set; }
    public DateTime? Finish { get; set; }
}