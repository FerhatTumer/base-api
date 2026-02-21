using System.ComponentModel.DataAnnotations;
using TaskManagement.Domain.Enums;
using DomainTaskStatus = TaskManagement.Domain.Enums.TaskStatus;

namespace TaskManagement.WebApi.Models.Requests.Tasks;

public record UpdateTaskStatusRequest
{
    [Required]
    [Range(1, int.MaxValue)]
    public int ProjectId { get; init; }

    [Required]
    public DomainTaskStatus Status { get; init; }
}