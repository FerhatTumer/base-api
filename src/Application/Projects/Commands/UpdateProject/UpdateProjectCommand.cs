using MediatR;
using TaskManagement.Application.Common.Models;

namespace TaskManagement.Application.Projects.Commands.UpdateProject;

public record UpdateProjectCommand(int ProjectId, string Name, string? Description) : IRequest<Result>;