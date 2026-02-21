using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskManagement.Domain.Aggregates.ProjectAggregate;
using TaskManagement.Domain.Aggregates.TeamAggregate;
using TaskManagement.Domain.Common;

namespace TaskManagement.Infrastructure.Persistence;

public sealed class ApplicationDbContext : DbContext
{
    private readonly IMediator _mediator;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IMediator mediator)
        : base(options)
    {
        _mediator = mediator;
    }

    public DbSet<Project> Projects => Set<Project>();

    public DbSet<TaskItem> TaskItems => Set<TaskItem>();

    public DbSet<Team> Teams => Set<Team>();

    public DbSet<TeamMember> TeamMembers => Set<TeamMember>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Ignore<DomainEvent>();
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return base.SaveChangesAsync(cancellationToken);
    }
}