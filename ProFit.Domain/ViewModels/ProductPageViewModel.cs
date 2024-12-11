using ProFit.Domain.Models;
using Type = ProFit.Domain.Enum.Type;

namespace ProFit.Domain.ViewModels;

public class ProductPageViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public double Price { get; set; }

    public Type Type { get; set; }
    public int ValidityPeriod { get; set; }
    public string PathImg { get; set; }
    public List<PictureProductViewModel> PictureProducts { get; set; }
}

public class PictureProductViewModel
{
    public string PathImg { get; set; }
}