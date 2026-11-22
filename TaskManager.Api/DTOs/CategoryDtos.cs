namespace TaskManager.Api.DTOs;

public record CategoryCreateDto(string Name);

public record CategoryUpdateDto(string Name);

public record CategoryResponseDto(int Id, string Name, int TaskCount);