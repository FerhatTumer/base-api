using System.ComponentModel.DataAnnotations;
using TaskManagement.Domain.Enums;

namespace TaskManagement.WebApi.Models.Requests.Teams;

public record AddTeamMemberRequest
{
    [Required]
    [Range(1, int.MaxValue)]
    public int UserId { get; init; }

    [Required]
    public TeamRole Role { get; init; }
}