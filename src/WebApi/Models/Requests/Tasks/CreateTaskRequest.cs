using System.ComponentModel.DataAnnotations;
using TaskManagement.Domain.Enums;

namespace TaskManagement.WebApi.Models.Requests.Tasks;

public record CreateTaskRequest
{
    [Required]
    [Range(1, int.MaxValue)]
    public int ProjectId { get; init; }

    [Required]
    [StringLength(200)]
    public string Title { get; init; } = string.Empty;

    [StringLength(2000)]
    public string? Description { get; init; }

    [Required]
    public Priority Priority { get; init; }

    public DateTimeOffset? DueDate { get; init; }

    public int? AssigneeId { get; init; }

    public decimal? EstimatedHours { get; init; }
}