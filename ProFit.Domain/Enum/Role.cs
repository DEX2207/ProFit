using System.ComponentModel.DataAnnotations;

namespace ProFit.Domain.Enum;

public enum Role
{
    [Display(Name = "Пользователь")]
    User=0,
    [Display(Name = "Модератор")]
    Moderator=1,
    [Display(Name = "Администратор")]
    Admin=2,
}