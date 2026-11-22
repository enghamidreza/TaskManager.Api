using FluentValidation;
using TaskManager.Api.DTOs;

namespace TaskManager.Api.Validators;

public class CategoryCreateDtoValidator : AbstractValidator<CategoryCreateDto>
{
    public CategoryCreateDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("نام دسته‌بندی الزامی است.")
            .MaximumLength(50).WithMessage("نام دسته‌بندی نباید بیشتر از ۵۰ کاراکتر باشد.");
    }
}

public class CategoryUpdateDtoValidator : AbstractValidator<CategoryUpdateDto>
{
    public CategoryUpdateDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("نام دسته‌بندی الزامی است.")
            .MaximumLength(50).WithMessage("نام دسته‌بندی نباید بیشتر از ۵۰ کاراکتر باشد.");
    }
}