using Microsoft.AspNetCore.Mvc;
using TaskManagement.Application.Common.Models;
using TaskManagement.WebApi.Models.Responses;

namespace TaskManagement.WebApi.Extensions;

public static class ResultExtensions
{
    public static ActionResult ToActionResult<T>(this Result<T> result)
    {
        if (result.IsSuccess)
        {
            return new OkObjectResult(ApiResponse<T>.Success(result.Value!));
        }

        return new BadRequestObjectResult(ApiResponse.Failure(result.Error.Message));
    }

    public static ActionResult ToActionResult(this Result result)
    {
        if (result.IsSuccess)
        {
            return new OkObjectResult(ApiResponse.SuccessResponse());
        }

        return new BadRequestObjectResult(ApiResponse.Failure(result.Error.Message));
    }
}