using System.ComponentModel.DataAnnotations.Schema;
using ProFit.Domain.Enum;

namespace ProFit.Domain.ModelsDb;

[Table("Basket")]

public class BasketDb
{
    [Column("id")] public Guid Id { get; set; }
    [Column("idUser")] public Guid IdUser { get; set; }
    [Column("idProduct")] public Guid IdProduct { get; set; }
    [Column("totalPrice")] public double TotalPrice { get; set; }
    [Column("status")] public Status Status { get; set; }
}