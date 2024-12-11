using FluentValidation;
using ProFit.Domain.Models;

namespace ProFit.Domain.Validators;

public class PictureProductValidator:AbstractValidator<PictureProduct>
{
    public PictureProductValidator()
    {
        RuleFor(d => d.PathIMG)
            .NotEmpty().WithMessage("Ссылка на картинку обязательна");
        RuleFor(d => d.IdProduct)
            .NotEmpty().WithMessage("Идентификатор товара обязателен");
    }
}