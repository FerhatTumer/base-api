using System.ComponentModel.DataAnnotations;

namespace TaskManagement.WebApi.Models.Requests.Tasks;

public record AssignTaskRequest
{
    [Required]
    [Range(1, int.MaxValue)]
    public int ProjectId { get; init; }

    [Required]
    [Range(1, int.MaxValue)]
    public int AssigneeId { get; init; }
}