using System.ComponentModel.DataAnnotations.Schema;

namespace ProFit.Domain.ModelsDb;

[Table("PictureProduct")]

public class PictureProductDb
{
    [Column("id")] public Guid Id { get; set; }
    [Column("pathIMG")] public string PathIMG { get; set; }
    [Column("idProduct")] public Guid IdProduct { get; set; }
}