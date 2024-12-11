using System.ComponentModel.DataAnnotations.Schema;
using Type = ProFit.Domain.Enum.Type;

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
    [Column("type")] public Type Type { get; set; }
}