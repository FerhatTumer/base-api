using TaskManagement.Application.Common.Exceptions;
using TaskManagement.WebApi.Models.Responses;

namespace TaskManagement.WebApi.Middleware;

public sealed class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
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
            _logger.LogError(ex, "An unhandled exception occurred");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        (int status, string title, string detail) = exception switch
        {
            ValidationException => (
                StatusCodes.Status400BadRequest,
                "Validation Error",
                "One or more validation errors occurred"),
            NotFoundException notFoundEx => (
                StatusCodes.Status404NotFound,
                "Not Found",
                notFoundEx.Message),
            ForbiddenAccessException => (
                StatusCodes.Status403Forbidden,
                "Forbidden",
                "Access denied"),
            UnauthorizedAccessException => (
                StatusCodes.Status401Unauthorized,
                "Unauthorized",
                "Authentication required"),
            _ => (
                StatusCodes.Status500InternalServerError,
                "Internal Server Error",
                "An error occurred while processing your request")
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = status;

        if (exception is ValidationException validationException)
        {
            ValidationErrorResponse validationResponse = ValidationErrorResponse.Failure("Validation failed", validationException.Errors);
            await context.Response.WriteAsJsonAsync(validationResponse);
            return;
        }

        ErrorResponse response = new(
            type: $"https://httpstatuses.com/{status}",
            title: title,
            status: status,
            detail: detail,
            instance: context.Request.Path,
            traceId: context.TraceIdentifier);

        await context.Response.WriteAsJsonAsync(response);
    }
}