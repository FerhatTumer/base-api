using System.ComponentModel.DataAnnotations;

namespace TaskManagement.WebApi.Models.Requests.Auth;

public record LoginRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; init; } = string.Empty;

    [Required]
    [StringLength(100, MinimumLength = 8)]
    public string Password { get; init; } = string.Empty;
}