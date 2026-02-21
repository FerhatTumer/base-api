using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Application.Common.Models;
using TaskManagement.WebApi.Models.Responses;

namespace TaskManagement.WebApi.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[Produces("application/json")]
public abstract class BaseApiController : ControllerBase
{
    private ISender? _mediator;

    protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();

    protected ActionResult<ApiResponse<T>> OkResponse<T>(T data, string message = "Success")
    {
        return Ok(ApiResponse<T>.Success(data, message));
    }

    protected ActionResult<ApiResponse> OkResponse(string message = "Success")
    {
        return Ok(ApiResponse.SuccessResponse(message));
    }

    protected ActionResult<ApiResponse> BadRequestResponse(string message, IDictionary<string, string[]>? errors = null)
    {
        if (errors is not null)
        {
            return BadRequest(ValidationErrorResponse.Failure(message, errors));
        }

        return BadRequest(ApiResponse.Failure(message));
    }

    protected ActionResult<ApiResponse> NotFoundResponse(string message = "Resource not found")
    {
        return NotFound(ApiResponse.Failure(message));
    }

    protected ActionResult HandleResult<T>(Result<T> result)
    {
        if (result.IsSuccess)
        {
            return Ok(ApiResponse<T>.Success(result.Value!));
        }

        return BadRequest(ApiResponse.Failure(result.Error.Message));
    }

    protected ActionResult HandleResult(Result result)
    {
        if (result.IsSuccess)
        {
            return Ok(ApiResponse.SuccessResponse());
        }

        return BadRequest(ApiResponse.Failure(result.Error.Message));
    }
}