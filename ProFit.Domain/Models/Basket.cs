using ProFit.Domain.Enum;

namespace ProFit.Domain.Models;

public class Basket
{
    public Guid Id { get; set; }
    public Guid IdUser { get; set; } 
    public Guid IdProduct { get; set; }
    public double TotalPrice { get; set; }
    public Status Status { get; set; }
}