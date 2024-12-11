using Type = ProFit.Domain.Enum.Type;

namespace ProFit.Domain.ViewModels;

public class ListOfProductsViewModel
{
    public List<ProductForListOfProductsViewModel> Products { get; set; }
}

public class ProductForListOfProductsViewModel
{
    public Guid Id { get; set; }                       
    public string PathIMG { get; set; } 
    public string Name { get; set; }                 
    public double Price { get; set; }               
    public int ValidityPeriod { get; set; }
    public Type Type { get; set; }
}