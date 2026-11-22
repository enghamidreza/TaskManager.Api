using TaskManager.Api.Models;

namespace TaskManager.Api.DTOs;

public class TaskQueryParams
{
    private const int MaxPageSize = 50;
    private int _pageSize = 10;
    private int _page = 1;

    // فیلتر
    public TaskItemStatus? Status { get; set; }
    public TaskPriority? Priority { get; set; }
    public int? CategoryId { get; set; }
    public string? Search { get; set; }

    // مرتب‌سازی
    public string SortBy { get; set; } = "createdAt";
    public string SortDir { get; set; } = "desc";

    // صفحه‌بندی
    public int Page
    {
        get => _page;
        set => _page = value < 1 ? 1 : value;
    }

    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value < 1 ? 10 : Math.Min(value, MaxPageSize);
    }
}