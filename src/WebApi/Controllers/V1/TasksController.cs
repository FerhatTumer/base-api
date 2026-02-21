using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Application.Common.Models;
using TaskManagement.Application.Tasks.Commands.AssignTask;
using TaskManagement.Application.Tasks.Commands.CompleteTask;
using TaskManagement.Application.Tasks.Commands.CreateTask;
using TaskManagement.Application.Tasks.Commands.UpdateTask;
using TaskManagement.Application.Tasks.Commands.UpdateTaskStatus;
using TaskManagement.Application.Tasks.DTOs;
using TaskManagement.Application.Tasks.Queries.GetOverdueTasks;
using TaskManagement.Application.Tasks.Queries.GetTaskById;
using TaskManagement.Application.Tasks.Queries.GetTasksByAssignee;
using TaskManagement.Application.Tasks.Queries.GetTasksByProject;
using TaskManagement.WebApi.Models.Requests.Tasks;
using TaskManagement.WebApi.Models.Responses;

namespace TaskManagement.WebApi.Controllers.V1;

[ApiVersion("1.0")]
[Authorize]
public sealed class TasksController : BaseApiController
{
    [HttpPost]
    public async Task<ActionResult> CreateTask([FromBody] CreateTaskRequest request, CancellationToken cancellationToken)
    {
        CreateTaskCommand command = new(
            request.ProjectId,
            request.Title,
            request.Description,
            request.Priority,
            request.DueDate,
            request.AssigneeId,
            request.EstimatedHours);

        Result<int> result = await Mediator.Send(command, cancellationToken);
        return HandleResult(result);
    }

    [HttpPut("{id:int}/assign")]
    public async Task<ActionResult> AssignTask([FromRoute] int id, [FromBody] AssignTaskRequest request, CancellationToken cancellationToken)
    {
        AssignTaskCommand command = new(request.ProjectId, id, request.AssigneeId);
        Result result = await Mediator.Send(command, cancellationToken);
        return HandleResult(result);
    }

    [HttpPut("{id:int}/status")]
    public async Task<ActionResult> UpdateTaskStatus([FromRoute] int id, [FromBody] UpdateTaskStatusRequest request, CancellationToken cancellationToken)
    {
        UpdateTaskStatusCommand command = new(request.ProjectId, id, request.Status);
        Result result = await Mediator.Send(command, cancellationToken);
        return HandleResult(result);
    }

    [HttpPut("{id:int}/complete")]
    public async Task<ActionResult> CompleteTask([FromRoute] int id, [FromBody] CompleteTaskRequest request, CancellationToken cancellationToken)
    {
        CompleteTaskCommand command = new(request.ProjectId, id);
        Result result = await Mediator.Send(command, cancellationToken);
        return HandleResult(result);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateTask([FromRoute] int id, [FromBody] UpdateTaskRequest request, CancellationToken cancellationToken)
    {
        UpdateTaskCommand command = new(
            request.ProjectId,
            id,
            request.Title,
            request.Description,
            request.Priority,
            request.DueDate,
            request.EstimatedHours);

        Result result = await Mediator.Send(command, cancellationToken);
        return HandleResult(result);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ApiResponse<TaskDetailDto>>> GetTaskById([FromRoute] int id, CancellationToken cancellationToken)
    {
        GetTaskByIdQuery query = new(id);
        TaskDetailDto result = await Mediator.Send(query, cancellationToken);
        return OkResponse(result);
    }

    [HttpGet("project/{projectId:int}")]
    public async Task<ActionResult> GetTasksByProject([FromRoute] int projectId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, CancellationToken cancellationToken = default)
    {
        GetTasksByProjectQuery query = new(projectId, pageNumber, pageSize);
        PaginatedList<TaskListDto> result = await Mediator.Send(query, cancellationToken);
        return Ok(new PaginatedResponse<TaskListDto>(result.Items, result.PageNumber, pageSize, result.TotalCount));
    }

    [HttpGet("assignee/{assigneeId:int}")]
    public async Task<ActionResult> GetTasksByAssignee([FromRoute] int assigneeId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, CancellationToken cancellationToken = default)
    {
        GetTasksByAssigneeQuery query = new(assigneeId, pageNumber, pageSize);
        PaginatedList<TaskListDto> result = await Mediator.Send(query, cancellationToken);
        return Ok(new PaginatedResponse<TaskListDto>(result.Items, result.PageNumber, pageSize, result.TotalCount));
    }

    [HttpGet("overdue")]
    public async Task<ActionResult> GetOverdueTasks([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, CancellationToken cancellationToken = default)
    {
        GetOverdueTasksQuery query = new(pageNumber, pageSize);
        PaginatedList<TaskListDto> result = await Mediator.Send(query, cancellationToken);
        return Ok(new PaginatedResponse<TaskListDto>(result.Items, result.PageNumber, pageSize, result.TotalCount));
    }
}