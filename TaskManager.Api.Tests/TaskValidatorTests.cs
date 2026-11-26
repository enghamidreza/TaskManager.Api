using TaskManager.Api.DTOs;
using TaskManager.Api.Models;
using TaskManager.Api.Validators;

namespace TaskManager.Api.Tests;

public class TaskValidatorTests
{
    private readonly TaskCreateDtoValidator _validator = new();

    // این یک متد است :fact
    [Fact]
    public void Title_Empty_IsInvalid()
    {
        var dto = new TaskCreateDto("", null, TaskPriority.Low, null, null);

        var result = _validator.Validate(dto);

        Assert.False(result.IsValid);
    }

    [Fact]
    public void ValidDto_IsValid()
    {
        var dto = new TaskCreateDto("کار درست", null, TaskPriority.Low, null, null);

        var result = _validator.Validate(dto);

        Assert.True(result.IsValid);
    }
}