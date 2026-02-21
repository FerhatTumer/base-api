using FluentAssertions;
using TaskManagement.Domain.Aggregates.ProjectAggregate;
using TaskManagement.Domain.Aggregates.TeamAggregate;
using TaskManagement.Domain.Common;
using TaskManagement.Domain.Enums;
using Xunit;

namespace TaskManagement.Domain.UnitTests;

public sealed class ProjectAndTeamDomainTests
{
    [Fact]
    public void CreateProject_WithTrimmedName_CreatesSuccessfully()
    {
        Project project = Project.Create("  Core Platform  ", "desc", 42);

        project.Name.Should().Be("Core Platform");
        project.OwnerId.Should().Be(42);
        project.Status.Should().Be(ProjectStatus.Active);
    }

    [Fact]
    public void ArchiveProject_WithActiveTasks_ThrowsDomainException()
    {
        Project project = Project.Create("Project A", null, 1);
        DateTimeOffset dueDate = DateTimeOffset.UtcNow.AddDays(3);

        project.AddTask("Task 1", null, Priority.High, dueDate, 10, 2.5m);

        Action act = () => project.Archive();

        act.Should().Throw<DomainException>()
            .WithMessage("*active tasks*");
    }

    [Fact]
    public void TeamCreate_InitializesLeaderAsMember()
    {
        Team team = Team.Create("Backend", "Team description", 100);

        team.LeaderId.Should().Be(100);
        team.Members.Should().ContainSingle(m => m.UserId == 100 && m.Role == TeamRole.Leader);
    }

    [Fact]
    public void AddMember_WhenDuplicateUser_ThrowsDomainException()
    {
        Team team = Team.Create("QA", null, 77);
        team.AddMember(88, TeamRole.Member);

        Action act = () => team.AddMember(88, TeamRole.Member);

        act.Should().Throw<DomainException>()
            .WithMessage("*cannot be added twice*");
    }
}
