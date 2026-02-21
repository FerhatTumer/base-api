using System.ComponentModel.DataAnnotations;

namespace TaskManagement.WebApi.Models.Requests.Teams;

public record ChangeTeamLeaderRequest
{
    [Required]
    [Range(1, int.MaxValue)]
    public int NewLeaderId { get; init; }
}