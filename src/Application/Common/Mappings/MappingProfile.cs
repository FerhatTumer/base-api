using AutoMapper;
using TaskManagement.Application.Projects.DTOs;
using TaskManagement.Application.Tasks.DTOs;
using TaskManagement.Application.Teams.DTOs;
using TaskManagement.Domain.Aggregates.ProjectAggregate;
using TaskManagement.Domain.Aggregates.TeamAggregate;

namespace TaskManagement.Application.Common.Mappings;

public sealed class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Project, ProjectDto>();

        CreateMap<Project, ProjectListDto>()
            .ForMember(d => d.TaskCount, opt => opt.MapFrom(s => s.TaskItems.Count));

        CreateMap<Project, ProjectDetailDto>()
            .ForMember(d => d.TaskCount, opt => opt.MapFrom(s => s.TaskItems.Count))
            .ForMember(d => d.Tasks, opt => opt.MapFrom(s => s.TaskItems));

        CreateMap<TaskItem, TaskDto>();
        CreateMap<TaskItem, TaskListDto>();
        CreateMap<TaskItem, TaskDetailDto>();

        CreateMap<TeamMember, TeamMemberDto>();
        CreateMap<Team, TeamDto>();

        CreateMap<Team, TeamListDto>()
            .ForMember(d => d.MemberCount, opt => opt.MapFrom(s => s.Members.Count(m => !m.IsDeleted)));

        CreateMap<Team, TeamDetailDto>()
            .ForMember(d => d.Members, opt => opt.MapFrom(s => s.Members.Where(m => !m.IsDeleted)));
    }
}