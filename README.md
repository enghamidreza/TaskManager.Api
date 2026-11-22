# Task Manager API

یک REST API برای مدیریت وظایف شخصی، ساخته‌شده با ASP.NET Core 9.

## امکانات
- ثبت‌نام و ورود با JWT
- CRUD وظایف و دسته‌بندی‌ها (مخصوص هر کاربر)
- صفحه‌بندی، فیلتر، جستجو و مرتب‌سازی
- اعتبارسنجی با FluentValidation
- مدیریت خطای سراسری (ProblemDetails)
- مستندات Swagger

## تکنولوژی‌ها
C#، ASP.NET Core 9، Entity Framework Core، SQL Server، Identity، JWT، FluentValidation، Swagger

## نحوه اجرا
1. مخزن را کلون کنید
2. Connection String را در `appsettings.json` تنظیم کنید
3. تنظیمات JWT را در User Secrets بگذارید:
