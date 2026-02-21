using System.ComponentModel.DataAnnotations;

namespace TaskManagement.WebApi.Models.Requests.Projects;

public record CreateProjectRequest
{
    [Required(ErrorMessage = "Project name is required")]
    [StringLength(200, ErrorMessage = "Name cannot exceed 200 characters")]
    public string Name { get; init; } = string.Empty;

    [StringLength(2000, ErrorMessage = "Description cannot exceed 2000 characters")]
    public string? Description { get; init; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Owner ID must be greater than 0")]
    public int OwnerId { get; init; }
}