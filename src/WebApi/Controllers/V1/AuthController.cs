using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.WebApi.Models.Requests.Auth;
using TaskManagement.WebApi.Models.Responses;

namespace TaskManagement.WebApi.Controllers.V1;

[ApiVersion("1.0")]
[AllowAnonymous]
public sealed class AuthController : BaseApiController
{
    [HttpPost("register")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationErrorResponse), StatusCodes.Status400BadRequest)]
    public ActionResult Register([FromBody] RegisterRequest request)
    {
        _ = request;
        return BadRequest(ApiResponse.Failure("Authentication flow is not implemented in Application layer yet."));
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public ActionResult Login([FromBody] LoginRequest request)
    {
        _ = request;
        return BadRequest(ApiResponse.Failure("Authentication flow is not implemented in Application layer yet."));
    }

    [HttpPost("refresh")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public ActionResult RefreshToken([FromBody] RefreshTokenRequest request)
    {
        _ = request;
        return BadRequest(ApiResponse.Failure("Authentication flow is not implemented in Application layer yet."));
    }

    [Authorize]
    [HttpPost("revoke")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    public ActionResult<ApiResponse> RevokeToken()
    {
        return OkResponse("Token revoked.");
    }
}