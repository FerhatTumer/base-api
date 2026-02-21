using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TaskManagement.Application.Common.Exceptions;
using TaskManagement.WebApi.Models.Responses;

namespace TaskManagement.WebApi.Filters;

public sealed class ApiExceptionFilterAttribute : ExceptionFilterAttribute
{
    public override void OnException(ExceptionContext context)
    {
        if (context.Exception is ValidationException validationException)
        {
            context.Result = new BadRequestObjectResult(
                ValidationErrorResponse.Failure("Validation failed", validationException.Errors));
            context.ExceptionHandled = true;
            return;
        }

        if (context.Exception is NotFoundException notFoundException)
        {
            context.Result = new NotFoundObjectResult(ApiResponse.Failure(notFoundException.Message));
            context.ExceptionHandled = true;
            return;
        }

        if (context.Exception is ForbiddenAccessException)
        {
            context.Result = new ObjectResult(ApiResponse.Failure("Access denied"))
            {
                StatusCode = StatusCodes.Status403Forbidden
            };
            context.ExceptionHandled = true;
        }
    }
}