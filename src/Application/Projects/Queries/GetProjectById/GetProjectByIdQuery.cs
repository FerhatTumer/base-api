using MediatR;
using TaskManagement.Application.Projects.DTOs;

namespace TaskManagement.Application.Projects.Queries.GetProjectById;

public record GetProjectByIdQuery(int ProjectId) : IRequest<ProjectDetailDto>;