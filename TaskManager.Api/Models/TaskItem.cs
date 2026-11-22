namespace TaskManager.Api.Models;

public enum TaskItemStatus
{
    Todo,  // 0
    InProgress,// 1
    Done// 2
}

public enum TaskPriority
{
    Low,// 0
    Medium,// 1
    High// 2
}

public class TaskItem
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    // مقدار پیش فرض 
    public TaskItemStatus Status { get; set; } = TaskItemStatus.Todo;
    public TaskPriority Priority { get; set; } = TaskPriority.Medium;
    public DateTime? DueDate { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public string UserId { get; set; } = string.Empty;
    // هر تسک مربوط ب یک یوزره 
    public AppUser? User { get; set; }

    public int? CategoryId { get; set; }
    // هر تسک مربوط به یک دسته بندیه 
    public Category? Category { get; set; }
}