using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using TaskManagement.Application.Common.Interfaces;

namespace TaskManagement.Infrastructure.Services;

public sealed class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public int? UserId => GetUserIdFromClaims();

    public string? UserName => _httpContextAccessor.HttpContext?.User?.Identity?.Name;

    public bool IsAuthenticated => _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;

    private int? GetUserIdFromClaims()
    {
        string? userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return int.TryParse(userIdClaim, out int userId) ? userId : null;
    }
}