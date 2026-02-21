using System.ComponentModel.DataAnnotations;

namespace TaskManagement.WebApi.Models.Requests.Tasks;

public record CompleteTaskRequest
{
    [Required]
    [Range(1, int.MaxValue)]
    public int ProjectId { get; init; }
}