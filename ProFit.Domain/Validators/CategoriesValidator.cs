using FluentValidation;
using ProFit.Domain.Models;

namespace ProFit.Domain.Validators;

public class CategoriesValidator:AbstractValidator<Categories>
{
    public CategoriesValidator()
    {
        RuleFor(d => d.Type)
            .NotEmpty().WithMessage("Поле не должно быть пустым");
        RuleFor(d=>d.Name)
            .NotEmpty().WithMessage("Название категории обязательно");
        RuleFor(d => d.PathIMG)
            .NotEmpty().WithMessage("Ссылка на картинку обязательна");
    }
}