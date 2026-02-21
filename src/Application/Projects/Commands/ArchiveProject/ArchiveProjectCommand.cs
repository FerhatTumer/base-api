using MediatR;
using TaskManagement.Application.Common.Models;

namespace TaskManagement.Application.Projects.Commands.ArchiveProject;

public record ArchiveProjectCommand(int ProjectId) : IRequest<Result>;