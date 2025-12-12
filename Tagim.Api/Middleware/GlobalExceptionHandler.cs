using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Tagim.Application.Exceptions;

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
                
            problemDetails.Extensions["errors"] = validationException.Errors
                .Select(e => new { Field = e.PropertyName, Error = e.ErrorMessage })
                .ToList();
        }
        else if (exception is NotFoundException notFoundException)
        {
            httpContext.Response.StatusCode = StatusCodes.Status404NotFound;
            problemDetails.Title = "Məlumat Tapılmadı";
            problemDetails.Status = StatusCodes.Status404NotFound;
            problemDetails.Detail = notFoundException.Message;
        }
        else if (exception is UnauthorizedAccessException)
        {
            httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
            problemDetails.Title = "Giriş Qadağandır";
            problemDetails.Status = StatusCodes.Status401Unauthorized;
            problemDetails.Detail = "Bu əməliyyatı yerinə yetirmək üçün sistemə daxil olmalısınız.";
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