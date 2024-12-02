using System.Data;
using FluentValidation;
using ProFit.Domain.Models;

namespace ProFit.Domain.Validators;

public class ProductValidator:AbstractValidator<Product>
{
    public ProductValidator()
    {
        RuleFor(d => d.Price)
            .NotEmpty().WithMessage("Поле не должно быть пустым")
            .GreaterThan(0).WithMessage("Число должно быть положительным");
        RuleFor(d => d.PathIMG)
            .NotEmpty().WithMessage("Путь изображения обязателен");
        RuleFor(d => d.Name)
            .NotEmpty().WithMessage("Название продукта обязательно");
        RuleFor(d => d.Description)
            .NotEmpty().WithMessage("Описание обязательно");
        RuleFor(d => d.ValidityPeriod)
            .NotEmpty().WithMessage("Срок действия абонемента обязателен")
            .GreaterThan(0).WithMessage("Срок действия должен быть положительным");
    }
}