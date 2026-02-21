using MediatR;
using TaskManagement.Application.Common.Models;

namespace TaskManagement.Application.Projects.Commands.CreateProject;

public record CreateProjectCommand(string Name, string? Description, int OwnerId) : IRequest<Result<int>>;