using TaskManager.Api.DTOs;
using TaskManager.Api.Models;

namespace TaskManager.Api.Mappings;

public static class TaskMappings
{
    public static TaskResponseDto ToResponseDto(this TaskItem task) =>
        new(
            task.Id,
            task.Title,
            task.Description,
            task.Status,
            task.Priority,
            task.DueDate,
            task.CreatedAt,
            task.CategoryId,
            task.Category?.Name);

    public static TaskItem ToEntity(this TaskCreateDto dto, string userId) =>
        new()
        {
            Title = dto.Title,
            Description = dto.Description,
            Priority = dto.Priority,
            DueDate = dto.DueDate,
            CategoryId = dto.CategoryId,
            UserId = userId
        };
}