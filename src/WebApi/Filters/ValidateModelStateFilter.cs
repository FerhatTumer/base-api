using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TaskManagement.WebApi.Models.Responses;

namespace TaskManagement.WebApi.Filters;

public sealed class ValidateModelStateFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            Dictionary<string, string[]> errors = context.ModelState
                .Where(x => x.Value is not null && x.Value.Errors.Count > 0)
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value!.Errors.Select(x => x.ErrorMessage).ToArray());

            context.Result = new BadRequestObjectResult(ValidationErrorResponse.Failure("Validation failed", errors));
        }
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
    }
}