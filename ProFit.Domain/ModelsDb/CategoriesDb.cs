using System.ComponentModel.DataAnnotations.Schema;
using Type = ProFit.Domain.Enum.Type;

namespace ProFit.Domain.ModelsDb;

[Table("Categories")]
public class CategoriesDb
{
    [Column("id")] public Guid Id { get; set; }
    [Column ("name")] public string Name { get; set; }
    [Column("pathIMG")] public string PathIMG { get; set; }
    [Column("type")] public Type Type { get; set; }
}