using FluentValidation;
using ProFit.Domain.Models;

namespace ProFit.Domain.Validators;

public class UserValidator:AbstractValidator<User>
{
    public UserValidator()
    {
        RuleFor(d => d.Email)
            .NotEmpty().WithMessage("Email обязателен")
            .EmailAddress().WithMessage("Неверный формат Email");
        RuleFor(d => d.Password)
            .NotEmpty().WithMessage("Пароль обязателен")
            .MinimumLength(6).WithMessage("Пароль должен содержать не менее 6 символов");
    }
}