using TaskManagement.Domain.Common;

namespace TaskManagement.Domain.Aggregates.TeamAggregate;

public interface ITeamRepository : IRepository<Team, int>
{
}