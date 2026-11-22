namespace TaskManager.Api.Models;

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public string UserId { get; set; } = string.Empty;
    public AppUser? User { get; set; }

    public List<TaskItem> Tasks { get; set; } = new();
}