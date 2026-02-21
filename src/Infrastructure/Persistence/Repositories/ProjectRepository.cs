using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TaskManagement.Domain.Aggregates.ProjectAggregate;

namespace TaskManagement.Infrastructure.Persistence.Repositories;

public sealed class ProjectRepository : IProjectRepository
{
    private readonly ApplicationDbContext _context;

    public ProjectRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public Task AddAsync(Project entity, CancellationToken cancellationToken)
    {
        return _context.Projects.AddAsync(entity, cancellationToken).AsTask();
    }

    public void Update(Project entity)
    {
        _context.Projects.Update(entity);
    }

    public void Remove(Project entity)
    {
        _context.Projects.Remove(entity);
    }

    public Task<Project?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        return _context.Projects
            .Include(x => x.TaskItems)
            .AsSplitQuery()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Project>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _context.Projects
            .Include(x => x.TaskItems)
            .AsNoTracking()
            .AsSplitQuery()
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Project>> WhereAsync(Expression<Func<Project, bool>> predicate, CancellationToken cancellationToken)
    {
        return await _context.Projects
            .Include(x => x.TaskItems)
            .Where(predicate)
            .AsNoTracking()
            .AsSplitQuery()
            .ToListAsync(cancellationToken);
    }
}