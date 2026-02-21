using System.ComponentModel.DataAnnotations;

namespace TaskManagement.WebApi.Models.Requests.Auth;

public record RefreshTokenRequest
{
    [Required]
    public string RefreshToken { get; init; } = string.Empty;
}