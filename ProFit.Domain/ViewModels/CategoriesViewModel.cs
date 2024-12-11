using Type = ProFit.Domain.Enum.Type;

namespace ProFit.Domain.ViewModels;

public class CategoriesViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } 
    public string PathIMG { get; set; }
    public Type Type { get; set; }
}