using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TaskManager.Api.Data;
using TaskManager.Api.DTOs;
using TaskManager.Api.Mappings;
using TaskManager.Api.Models;

namespace TaskManager.Api.Controllers;
[Authorize]
[ApiController]   // قوانین مخصوص به api
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private readonly AppDbContext _context;

    // موقت: بعداً از توکن JWT می‌خوانیم
    //private const string UserId = "test-user-1";
    // شناسایی توکن از طریق یوزر ایدی 
    private string UserId =>
    User.FindFirstValue(ClaimTypes.NameIdentifier)
    ?? throw new UnauthorizedAccessException();

    public TasksController(AppDbContext context)
    {
        _context = context;
    }
    //کوئری از از url برمیداره :FromQuery
    [HttpGet]
    public async Task<ActionResult<PagedResult<TaskResponseDto>>> GetAll(
    [FromQuery] TaskQueryParams query)
    {
        var tasksQuery = _context.Tasks
            .Include(t => t.Category)
            .Where(t => t.UserId == UserId);

        // فیلترها
        if (query.Status is not null)
            tasksQuery = tasksQuery.Where(t => t.Status == query.Status);

        if (query.Priority is not null)
            tasksQuery = tasksQuery.Where(t => t.Priority == query.Priority);

        if (query.CategoryId is not null)
            tasksQuery = tasksQuery.Where(t => t.CategoryId == query.CategoryId);

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var search = query.Search.Trim();
            tasksQuery = tasksQuery.Where(t =>
                t.Title.Contains(search) ||
                (t.Description != null && t.Description.Contains(search)));
        }

        // مرتب‌سازی
        var desc = query.SortDir.Equals("desc", StringComparison.OrdinalIgnoreCase);

        tasksQuery = query.SortBy.ToLower() switch
        {
            "title" => desc ? tasksQuery.OrderByDescending(t => t.Title) : tasksQuery.OrderBy(t => t.Title),
            "duedate" => desc ? tasksQuery.OrderByDescending(t => t.DueDate) : tasksQuery.OrderBy(t => t.DueDate),
            "priority" => desc ? tasksQuery.OrderByDescending(t => t.Priority) : tasksQuery.OrderBy(t => t.Priority),
            "status" => desc ? tasksQuery.OrderByDescending(t => t.Status) : tasksQuery.OrderBy(t => t.Status),
            _ => desc ? tasksQuery.OrderByDescending(t => t.CreatedAt) : tasksQuery.OrderBy(t => t.CreatedAt)
        };

        // ترتیب ثانویه برای قطعی شدن نتیجه
        //یش فرض صعودیه :ThenBy
        tasksQuery = ((IOrderedQueryable<TaskItem>)tasksQuery).ThenBy(t => t.Id);

        // تعداد کل (قبل از Skip/Take)
        var totalCount = await tasksQuery.CountAsync();

        // صفحه‌بندی
        var tasks = await tasksQuery
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToListAsync();

        var result = new PagedResult<TaskResponseDto>
        {
            Items = tasks.Select(t => t.ToResponseDto()).ToList(),
            TotalCount = totalCount,
            Page = query.Page,
            PageSize = query.PageSize
        };

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TaskResponseDto>> GetById(int id)
    {
        var task = await _context.Tasks
            .Include(t => t.Category)
            .FirstOrDefaultAsync(t => t.Id == id && t.UserId == UserId);

        if (task is null)
            return NotFound();

        return Ok(task.ToResponseDto());
    }

    [HttpPost]
    public async Task<ActionResult<TaskResponseDto>> Create(TaskCreateDto dto)
    {
        var task = dto.ToEntity(UserId);

        if (dto.CategoryId is not null)
        {
            var categoryExists = await _context.Categories
                .AnyAsync(c => c.Id == dto.CategoryId && c.UserId == UserId);

            if (!categoryExists)
                return BadRequest("دسته‌بندی انتخاب‌شده وجود ندارد.");
        }

        _context.Tasks.Add(task);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = task.Id }, task.ToResponseDto());
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, TaskUpdateDto dto)
    {
        var task = await _context.Tasks
            .FirstOrDefaultAsync(t => t.Id == id && t.UserId == UserId);

        if (task is null)
            return NotFound();

        if (dto.CategoryId is not null)
        {
            var categoryExists = await _context.Categories
                .AnyAsync(c => c.Id == dto.CategoryId && c.UserId == UserId);

            if (!categoryExists)
                return BadRequest("دسته‌بندی انتخاب‌شده وجود ندارد.");
        }

        task.Title = dto.Title;
        task.Description = dto.Description;
        task.Status = dto.Status;
        task.Priority = dto.Priority;
        task.DueDate = dto.DueDate;
        task.CategoryId = dto.CategoryId;

        await _context.SaveChangesAsync();
        // انجام شد ولی ب کاربر نمایش نده
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var task = await _context.Tasks
            .FirstOrDefaultAsync(t => t.Id == id && t.UserId == UserId);

        if (task is null)
            return NotFound();

        _context.Tasks.Remove(task);
        await _context.SaveChangesAsync();

        return NoContent();
    }

}