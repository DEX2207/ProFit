using ProFit.Domain.Enum;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProFit.Domain.ModelsDb;

[Table("User")]
public class UserDb
{
    [Column("id")] public Guid Id { get; set; }
    [Column("login")] public string Login { get; set; }
    [Column("password")] public string Password { get; set; }
    [Column("email")] public string Email { get; set; }
    [Column("createdAt")] public DateTime CreatedAt { get; set; }
    [Column("role")] public Role Role { get; set; }
}