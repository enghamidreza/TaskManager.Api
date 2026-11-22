using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TaskManager.Api.Data;
using TaskManager.Api.DTOs;
using TaskManager.Api.Models;

namespace TaskManager.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly AppDbContext _context;

    public CategoriesController(AppDbContext context)
    {
        _context = context;
    }

    private string UserId =>
        User.FindFirstValue(ClaimTypes.NameIdentifier)
        ?? throw new UnauthorizedAccessException();

    [HttpGet]
    public async Task<ActionResult<List<CategoryResponseDto>>> GetAll()
    {
        var categories = await _context.Categories
            .Where(c => c.UserId == UserId)
            .OrderBy(c => c.Name)
            .Select(c => new CategoryResponseDto(c.Id, c.Name, c.Tasks.Count))
            .ToListAsync();

        return Ok(categories);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CategoryResponseDto>> GetById(int id)
    {
        var category = await _context.Categories
            .Where(c => c.Id == id && c.UserId == UserId)
            .Select(c => new CategoryResponseDto(c.Id, c.Name, c.Tasks.Count))
            .FirstOrDefaultAsync();

        if (category is null)
            return NotFound();

        return Ok(category);
    }

    [HttpPost]
    public async Task<ActionResult<CategoryResponseDto>> Create(CategoryCreateDto dto)
    {
        var name = dto.Name.Trim();

        var exists = await _context.Categories
            .AnyAsync(c => c.UserId == UserId && c.Name == name);

        if (exists)
            return Conflict("دسته‌بندی با این نام قبلاً ساخته شده است.");

        var category = new Category { Name = name, UserId = UserId };

        _context.Categories.Add(category);
        await _context.SaveChangesAsync();

        //CreatedAtAction یعنی برو به اادرس در اکشن مربوطه
        return CreatedAtAction(nameof(GetById), new { id = category.Id },
            new CategoryResponseDto(category.Id, category.Name, 0));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, CategoryUpdateDto dto)
    {
        var category = await _context.Categories
            .FirstOrDefaultAsync(c => c.Id == id && c.UserId == UserId);

        if (category is null)
            return NotFound();

        var name = dto.Name.Trim();

        var duplicate = await _context.Categories
            .AnyAsync(c => c.UserId == UserId && c.Name == name && c.Id != id);

        if (duplicate)
            return Conflict("دسته‌بندی با این نام قبلاً ساخته شده است.");

        category.Name = name;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var category = await _context.Categories
            .FirstOrDefaultAsync(c => c.Id == id && c.UserId == UserId);

        if (category is null)
            return NotFound();

        // وظایف این دسته را بدون دسته‌بندی کن
        await _context.Tasks
            .Where(t => t.CategoryId == id && t.UserId == UserId)
            .ExecuteUpdateAsync(s => s.SetProperty(t => t.CategoryId, (int?)null));

        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();
        // عملیات موفق ولی چیزی برای نمایش نمیزاریم 
        return NoContent();
    }
}