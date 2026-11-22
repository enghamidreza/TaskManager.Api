using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace TaskManager.Api.Middleware;

//برای پیدا کردن خطای غیر منتظره  
public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        // جزئیات کامل فقط در لاگ سرور
        _logger.LogError(exception,
            "Unhandled exception. TraceId: {TraceId}", httpContext.TraceIdentifier);

        var problem = new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "خطای داخلی سرور رخ داد.",
            Detail = "مشکلی پیش آمد. لطفاً بعداً دوباره تلاش کنید.",
            Extensions = { ["traceId"] = httpContext.TraceIdentifier }
        };

        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        await httpContext.Response.WriteAsJsonAsync(problem, cancellationToken);

        return true; // یعنی خطا مدیریت شد
    }
}