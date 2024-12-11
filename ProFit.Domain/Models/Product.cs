using Type = ProFit.Domain.Enum.Type;

namespace ProFit.Domain.Models;

public class Product
{
    public Guid Id { get; set; }                       
    public string PathIMG { get; set; } 
    public string Name { get; set; }                 
    public string Description { get; set; } 
    public double Price { get; set; }               
    public int ValidityPeriod { get; set; }
    public Type Type { get; set; }
}