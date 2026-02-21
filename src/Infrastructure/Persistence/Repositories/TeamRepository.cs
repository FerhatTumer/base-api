using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TaskManagement.Domain.Aggregates.TeamAggregate;

namespace TaskManagement.Infrastructure.Persistence.Repositories;

public sealed class TeamRepository : ITeamRepository
{
    private readonly ApplicationDbContext _context;

    public TeamRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public Task AddAsync(Team entity, CancellationToken cancellationToken)
    {
        return _context.Teams.AddAsync(entity, cancellationToken).AsTask();
    }

    public void Update(Team entity)
    {
        _context.Teams.Update(entity);
    }

    public void Remove(Team entity)
    {
        _context.Teams.Remove(entity);
    }

    public Task<Team?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        return _context.Teams
            .Include(x => x.Members)
            .AsSplitQuery()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Team>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _context.Teams
            .Include(x => x.Members)
            .AsNoTracking()
            .AsSplitQuery()
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Team>> WhereAsync(Expression<Func<Team, bool>> predicate, CancellationToken cancellationToken)
    {
        return await _context.Teams
            .Include(x => x.Members)
            .Where(predicate)
            .AsNoTracking()
            .AsSplitQuery()
            .ToListAsync(cancellationToken);
    }
}