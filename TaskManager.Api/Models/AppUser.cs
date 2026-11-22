using Microsoft.AspNetCore.Identity;

namespace TaskManager.Api.Models;

public class AppUser : IdentityUser
{
    public string FullName { get; set; } = string.Empty;

    //یک کاربر میتونه تسک بزنه و دسته بندی ایجاد کنه :رابطه  1 ب چند 
    // برای ساختن یک لیست اولیه حالی :new
    //اگر نباشه خطای نال رفنس میده 

    public List<TaskItem> Tasks { get; set; } = new();
    public List<Category> Categories { get; set; } = new();
}