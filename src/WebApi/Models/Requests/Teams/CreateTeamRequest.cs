using System.ComponentModel.DataAnnotations;

namespace TaskManagement.WebApi.Models.Requests.Teams;

public record CreateTeamRequest
{
    [Required]
    [StringLength(100)]
    public string Name { get; init; } = string.Empty;

    [StringLength(1000)]
    public string? Description { get; init; }

    [Required]
    [Range(1, int.MaxValue)]
    public int LeaderId { get; init; }
}