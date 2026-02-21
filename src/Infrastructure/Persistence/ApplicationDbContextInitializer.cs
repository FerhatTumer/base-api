using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TaskManagement.Domain.Aggregates.ProjectAggregate;

namespace TaskManagement.Infrastructure.Persistence;

public sealed class ApplicationDbContextInitializer
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<ApplicationDbContextInitializer> _logger;

    public ApplicationDbContextInitializer(ApplicationDbContext context, ILogger<ApplicationDbContextInitializer> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task InitializeAsync()
    {
        try
        {
            await _context.Database.MigrateAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while migrating the database.");
            throw;
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    private async Task TrySeedAsync()
    {
        if (!await _context.Projects.AnyAsync())
        {
            Project project = Project.Create(
                "Sample Project",
                "This is a sample project for development",
                1);

            await _context.Projects.AddAsync(project);
            await _context.SaveChangesAsync();
        }
    }
}