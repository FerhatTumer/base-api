using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Application.Common.Models;
using TaskManagement.Application.Teams.Commands.AddTeamMember;
using TaskManagement.Application.Teams.Commands.ChangeTeamLeader;
using TaskManagement.Application.Teams.Commands.CreateTeam;
using TaskManagement.Application.Teams.Commands.RemoveTeamMember;
using TaskManagement.Application.Teams.DTOs;
using TaskManagement.Application.Teams.Queries.GetAllTeams;
using TaskManagement.Application.Teams.Queries.GetTeamById;
using TaskManagement.Application.Teams.Queries.GetTeamsByLeader;
using TaskManagement.WebApi.Models.Requests.Teams;
using TaskManagement.WebApi.Models.Responses;

namespace TaskManagement.WebApi.Controllers.V1;

[ApiVersion("1.0")]
[Authorize]
public sealed class TeamsController : BaseApiController
{
    [HttpPost]
    public async Task<ActionResult> CreateTeam([FromBody] CreateTeamRequest request, CancellationToken cancellationToken)
    {
        CreateTeamCommand command = new(request.Name, request.Description, request.LeaderId);
        Result<int> result = await Mediator.Send(command, cancellationToken);
        return HandleResult(result);
    }

    [HttpPost("{id:int}/members")]
    public async Task<ActionResult> AddTeamMember([FromRoute] int id, [FromBody] AddTeamMemberRequest request, CancellationToken cancellationToken)
    {
        AddTeamMemberCommand command = new(id, request.UserId, request.Role);
        Result result = await Mediator.Send(command, cancellationToken);
        return HandleResult(result);
    }

    [HttpDelete("{id:int}/members/{userId:int}")]
    public async Task<ActionResult> RemoveTeamMember([FromRoute] int id, [FromRoute] int userId, [FromQuery] int? newLeaderId, CancellationToken cancellationToken)
    {
        RemoveTeamMemberCommand command = new(id, userId, newLeaderId);
        Result result = await Mediator.Send(command, cancellationToken);
        return HandleResult(result);
    }

    [HttpPut("{id:int}/leader")]
    public async Task<ActionResult> ChangeTeamLeader([FromRoute] int id, [FromBody] ChangeTeamLeaderRequest request, CancellationToken cancellationToken)
    {
        ChangeTeamLeaderCommand command = new(id, request.NewLeaderId);
        Result result = await Mediator.Send(command, cancellationToken);
        return HandleResult(result);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ApiResponse<TeamDetailDto>>> GetTeamById([FromRoute] int id, CancellationToken cancellationToken)
    {
        GetTeamByIdQuery query = new(id);
        TeamDetailDto result = await Mediator.Send(query, cancellationToken);
        return OkResponse(result);
    }

    [HttpGet]
    public async Task<ActionResult> GetAllTeams([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, CancellationToken cancellationToken = default)
    {
        GetAllTeamsQuery query = new(pageNumber, pageSize);
        PaginatedList<TeamListDto> result = await Mediator.Send(query, cancellationToken);
        return Ok(new PaginatedResponse<TeamListDto>(result.Items, result.PageNumber, pageSize, result.TotalCount));
    }

    [HttpGet("leader/{leaderId:int}")]
    public async Task<ActionResult> GetTeamsByLeader([FromRoute] int leaderId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, CancellationToken cancellationToken = default)
    {
        GetTeamsByLeaderQuery query = new(leaderId, pageNumber, pageSize);
        PaginatedList<TeamListDto> result = await Mediator.Send(query, cancellationToken);
        return Ok(new PaginatedResponse<TeamListDto>(result.Items, result.PageNumber, pageSize, result.TotalCount));
    }
}