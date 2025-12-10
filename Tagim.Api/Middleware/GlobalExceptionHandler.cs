using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Tagim.Api.Middleware;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger = logger;

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "Xəta baş verdi: {Message}", exception.Message);

        var problemDetails = new ProblemDetails
        {
            Instance = httpContext.Request.Path
        };
        
        // 1. FluentValidation
        if (exception is ValidationException validationException)
        {
            httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            problemDetails.Title = "Validasiya Xətası";
            problemDetails.Status = StatusCodes.Status400BadRequest;
            problemDetails.Detail = "Göndərilən məlumatlarda səhv var.";
                
            // Səhvləri siyahı kimi əlavə edirik
            problemDetails.Extensions["errors"] = validationException.Errors
                .Select(e => new { Field = e.PropertyName, Error = e.ErrorMessage })
                .ToList();
        }
        // Other exceptions
        else
        {
            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            problemDetails.Title = "Server Xətası";
            problemDetails.Status = StatusCodes.Status500InternalServerError;
            problemDetails.Detail = "Daxili xəta baş verdi. Zəhmət olmasa adminlə əlaqə saxlayın.";
        }
        
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
        
        return true;
    }
}