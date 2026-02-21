using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Application.Common.Models;
using TaskManagement.Application.Projects.Commands.ArchiveProject;
using TaskManagement.Application.Projects.Commands.CreateProject;
using TaskManagement.Application.Projects.Commands.UpdateProject;
using TaskManagement.Application.Projects.DTOs;
using TaskManagement.Application.Projects.Queries.GetAllProjects;
using TaskManagement.Application.Projects.Queries.GetProjectById;
using TaskManagement.Application.Projects.Queries.GetProjectsByOwner;
using TaskManagement.WebApi.Models.Requests.Projects;
using TaskManagement.WebApi.Models.Responses;

namespace TaskManagement.WebApi.Controllers.V1;

[ApiVersion("1.0")]
[Authorize]
public sealed class ProjectsController : BaseApiController
{
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<int>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> CreateProject([FromBody] CreateProjectRequest request, CancellationToken cancellationToken)
    {
        CreateProjectCommand command = new(request.Name, request.Description, request.OwnerId);
        Result<int> result = await Mediator.Send(command, cancellationToken);
        return HandleResult(result);
    }

    [HttpPut("{id:int}/archive")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> ArchiveProject([FromRoute] int id, CancellationToken cancellationToken)
    {
        ArchiveProjectCommand command = new(id);
        Result result = await Mediator.Send(command, cancellationToken);
        return HandleResult(result);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> UpdateProject([FromRoute] int id, [FromBody] UpdateProjectRequest request, CancellationToken cancellationToken)
    {
        UpdateProjectCommand command = new(id, request.Name, request.Description);
        Result result = await Mediator.Send(command, cancellationToken);
        return HandleResult(result);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<ProjectDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<ProjectDetailDto>>> GetProjectById([FromRoute] int id, CancellationToken cancellationToken)
    {
        GetProjectByIdQuery query = new(id);
        ProjectDetailDto result = await Mediator.Send(query, cancellationToken);
        return OkResponse(result);
    }

    [HttpGet]
    [ProducesResponseType(typeof(PaginatedResponse<ProjectListDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult> GetAllProjects([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, CancellationToken cancellationToken = default)
    {
        GetAllProjectsQuery query = new(pageNumber, pageSize);
        PaginatedList<ProjectListDto> result = await Mediator.Send(query, cancellationToken);
        PaginatedResponse<ProjectListDto> response = new(result.Items, result.PageNumber, pageSize, result.TotalCount);
        return Ok(response);
    }

    [HttpGet("owner/{ownerId:int}")]
    [ProducesResponseType(typeof(PaginatedResponse<ProjectListDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult> GetProjectsByOwner([FromRoute] int ownerId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, CancellationToken cancellationToken = default)
    {
        GetProjectsByOwnerQuery query = new(ownerId, pageNumber, pageSize);
        PaginatedList<ProjectListDto> result = await Mediator.Send(query, cancellationToken);
        PaginatedResponse<ProjectListDto> response = new(result.Items, result.PageNumber, pageSize, result.TotalCount);
        return Ok(response);
    }
}