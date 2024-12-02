using FluentValidation;
using ProFit.Domain.Models;

namespace ProFit.Domain.Validators;

public class BasketValidator:AbstractValidator<Basket>
{
    public BasketValidator()
    {
        RuleFor(d => d.TotalPrice)
            .NotEmpty().WithMessage("Поле не должно быть пустым")
            .GreaterThan(0).WithMessage("Цена должна быть больше нуля");
        RuleFor(d => d.IdUser)
            .NotEmpty().WithMessage("Id пользователя обязателен");
        RuleFor(d => d.IdProduct)
            .NotEmpty().WithMessage("Id продукта обязателен");
        RuleFor(d => d.Status)
            .IsInEnum().WithMessage("Статус должен быть допустимым значением перечисления");
    }
}