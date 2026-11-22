using TaskManager.Api.Models;

namespace TaskManager.Api.DTOs;
//یک نوع تایپ کلاس هست که داده  ها رو داخل اش نگه میداره :record
//و خروجی اش ترو یا فالسه ک فرقش با تعریف با تایپ معمولی پراپرتیه 
public record TaskCreateDto(
    string Title,
    string? Description,
    TaskPriority Priority,
    DateTime? DueDate,
    int? CategoryId);

public record TaskUpdateDto(
    string Title,
    string? Description,
    TaskItemStatus Status,
    TaskPriority Priority,
    DateTime? DueDate,
    int? CategoryId);

public record TaskResponseDto(
    int Id,
    string Title,
    string? Description,
    TaskItemStatus Status,
    TaskPriority Priority,
    DateTime? DueDate,
    DateTime CreatedAt,
    int? CategoryId,
    string? CategoryName);