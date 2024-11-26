using System.ComponentModel.DataAnnotations.Schema;

namespace ProFit.Domain.ModelsDb;

[Table("Product")]
public class ProductDb
{
    [Column("id")] public Guid Id { get; set; }
    [Column("pathIMG")] public string PathIMG { get; set; }
    [Column("name")] public string Name { get; set; }
    [Column("Description")] public string Description { get; set; }
    [Column("price")] public double Price { get; set; }
    [Column("validityPeriod")] public int ValidityPeriod { get; set; }
}