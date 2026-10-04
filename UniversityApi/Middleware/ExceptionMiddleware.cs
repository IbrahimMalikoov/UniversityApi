using System.Text.Json;
using UniversityApi.Common;

namespace UniversityApi.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception occurred: {Message}", ex.Message);
            await HandleExceptionAsync(context);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;

        var response = ReturnResult<string>.Fail(
            "INTERNAL_SERVER_ERROR", 
            "An unexpected error occurred. Please contact administrator.", 
            StatusCodes.Status500InternalServerError);

        var json = JsonSerializer.Serialize(response);
        await context.Response.WriteAsync(json);
    }
}