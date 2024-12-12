using Type = ProFit.Domain.Enum.Type;

namespace ProFit.Domain.Filter;

public class ProductFilter
{
    public Type Type { get; set; }
    public double PriceAfultMin { get; set; }
    public double PriceAfultMax { get; set; }
    
}