using FluentValidation;
using TaskManager.Api.DTOs;

namespace TaskManager.Api.Validators;

// یک کلاس از کتابخانه FluentValidation هستش : AbstractValidator
// از ابسترک کلاس نمیتوان شیئ ساخت ولی میشه از اونها ارث بری کرد 
public class TaskCreateDtoValidator : AbstractValidator<TaskCreateDto>
{
    public TaskCreateDtoValidator()
    {
        // تعریف قوانین :RuleFor
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("عنوان الزامی است.")
            .MaximumLength(100).WithMessage("عنوان نباید بیشتر از ۱۰۰ کاراکتر باشد.");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("توضیحات نباید بیشتر از ۱۰۰۰ کاراکتر باشد.");

        RuleFor(x => x.Priority)
            .IsInEnum().WithMessage("اولویت نامعتبر است.");

        RuleFor(x => x.CategoryId)
            .GreaterThan(0).When(x => x.CategoryId is not null)
            .WithMessage("شناسه دسته‌بندی نامعتبر است.");
        RuleFor(x => x.DueDate)
        .Must(date => date!.Value.ToUniversalTime() > DateTime.UtcNow)
        .When(x => x.DueDate is not null)
        .WithMessage("تاریخ سررسید نمی‌تواند در گذشته باشد.");
    }
}

public class TaskUpdateDtoValidator : AbstractValidator<TaskUpdateDto>
{
    public TaskUpdateDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("عنوان الزامی است.")
            .MaximumLength(100).WithMessage("عنوان نباید بیشتر از ۱۰۰ کاراکتر باشد.");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("توضیحات نباید بیشتر از ۱۰۰۰ کاراکتر باشد.");

        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("وضعیت نامعتبر است.");

        RuleFor(x => x.Priority)
            .IsInEnum().WithMessage("اولویت نامعتبر است.");

        RuleFor(x => x.CategoryId)
            .GreaterThan(0).When(x => x.CategoryId is not null)
            .WithMessage("شناسه دسته‌بندی نامعتبر است.");
    }
}