using TaskManagement.Domain.Common;

namespace TaskManagement.Domain.Aggregates.ProjectAggregate;

public interface IProjectRepository : IRepository<Project, int>
{
}