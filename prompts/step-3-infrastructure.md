# Infrastructure Layer - Clean Architecture Prompt

You are a senior .NET software architect.

Generate a complete Infrastructure Layer using:

- Clean Architecture
- Domain-Driven Design (DDD)
- Entity Framework Core 10
- PostgreSQL
- AWS Services (S3, SES, Secrets Manager)
- .NET 10
- C#

The output must ONLY contain the Infrastructure Layer code.
No explanations. No comments outside code.
Follow the folder structure and rules exactly.

========================================
BUSINESS DOMAIN CONTEXT
========================================

This is a Task Management System Infrastructure that provides:

**Data Persistence:**
- PostgreSQL database via EF Core
- Repository implementations for Project and Team aggregates
- Unit of Work implementation
- DbContext with proper configuration
- Migrations support
- Audit fields (CreatedAt, UpdatedAt, IsDeleted)
- Soft delete query filters
- Optimistic concurrency with RowVersion

**External Services:**
- AWS S3 for file storage
- AWS SES for email notifications
- AWS Secrets Manager for configuration
- DateTime provider
- Current user service

**Domain Event Dispatching:**
- Intercept SaveChanges to dispatch domain events
- Clear domain events after dispatch
- Transaction-aware event publishing

========================================
ARCHITECTURE RULES
========================================

- Infrastructure layer must be inside: src/Infrastructure
- Depends on Application and Domain layers
- Implements all Application layer interfaces
- EF Core configuration via Fluent API (NO data annotations in Domain)
- Use separate configuration classes for each entity
- Repository pattern implementation
- Unit of Work pattern with transaction support
- PostgreSQL-specific features (JSONB, arrays, full-text search)
- Connection string from configuration
- Migration naming convention: YYYYMMDDHHMMSS_MigrationName
- All configurations must enforce Domain rules
- Shadow properties for EF-only fields (RowVersion)
- Value object conversion
- Enum to string conversion
- DateTimeOffset for all dates
- UTC timezone enforcement
- Soft delete global query filter
- No cascade delete (explicit deletion only)
- Index optimization for queries
- Foreign key naming convention: FK_{Table}_{ReferencedTable}_{Column}

========================================
FOLDER STRUCTURE
========================================

src
 └── Infrastructure
       ├── Persistence
       │     ├── ApplicationDbContext.cs
       │     ├── ApplicationDbContextInitializer.cs
       │     ├── Configurations
       │     │     ├── ProjectConfiguration.cs
       │     │     ├── TaskItemConfiguration.cs
       │     │     ├── TeamConfiguration.cs
       │     │     └── TeamMemberConfiguration.cs
       │     ├── Interceptors
       │     │     ├── AuditableEntityInterceptor.cs
       │     │     ├── SoftDeleteInterceptor.cs
       │     │     └── DispatchDomainEventsInterceptor.cs
       │     ├── Repositories
       │     │     ├── ProjectRepository.cs
       │     │     └── TeamRepository.cs
       │     └── UnitOfWork.cs
       │
       ├── Services
       │     ├── DateTimeProvider.cs
       │     ├── CurrentUserService.cs
       │     ├── EmailService.cs
       │     └── FileStorageService.cs
       │
       ├── AWS
       │     ├── S3Service.cs
       │     ├── SESService.cs
       │     └── SecretsManagerService.cs
       │
       ├── Identity
       │     ├── ApplicationUser.cs
       │     └── IdentityService.cs
       │
       └── DependencyInjection.cs

========================================
PACKAGE REFERENCES
========================================

Required NuGet packages (referenced in Infrastructure.csproj):

- Microsoft.EntityFrameworkCore (10.x or later)
- Microsoft.EntityFrameworkCore.Design (10.x or later)
- Npgsql.EntityFrameworkCore.PostgreSQL (10.x or later)
- Microsoft.EntityFrameworkCore.Tools (10.x or later)
- Microsoft.Extensions.Configuration.Abstractions
- Microsoft.Extensions.DependencyInjection.Abstractions
- Microsoft.AspNetCore.Http.Abstractions (for IHttpContextAccessor)
- AWSSDK.S3 (latest)
- AWSSDK.SimpleEmail (latest)
- AWSSDK.SecretsManager (latest)
- MediatR (13.x or later)

========================================
DBCONTEXT REQUIREMENTS
========================================

**ApplicationDbContext:**

Inherits: DbContext

DbSets:
- DbSet<Project> Projects
- DbSet<Team> Teams

Constructor:
- ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IMediator mediator) : base(options)
- Store mediator for domain event dispatching

Override OnModelCreating:
- Apply all configurations from current assembly
- Apply global query filter for soft delete: .HasQueryFilter(e => !e.IsDeleted)
- Configure value conversions
- Configure enum to string conversions
- Set default schema if needed

Override SaveChangesAsync:
- Call interceptors automatically (handled by AddInterceptor in DI)
- Return base.SaveChangesAsync(cancellationToken)

Important:
- Do NOT manually dispatch domain events in SaveChangesAsync
- Domain event dispatching is handled by DispatchDomainEventsInterceptor
- Interceptors are registered in DI configuration

========================================
ENTITY CONFIGURATIONS
========================================

**Configuration Base Pattern:**

Each entity has a separate configuration class implementing IEntityTypeConfiguration<T>

**ProjectConfiguration:**
- Table name: "Projects"
- Primary key: Id (auto-generated)
- Properties:
  * Name: required, max length 200, index
  * Description: optional, max length 2000
  * OwnerId: required, index
  * Status: enum to string conversion, max length 50
  * CreatedAt: required, default value UTC now
  * UpdatedAt: optional
  * IsDeleted: required, default false, index
  * RowVersion: shadow property, concurrency token, type: xmin for PostgreSQL
- Relationships:
  * HasMany Tasks (TaskItem) with HasForeignKey(t => t.ProjectId)
  * OnDelete: Restrict (no cascade)
- Indexes:
  * IX_Projects_OwnerId
  * IX_Projects_Status
  * IX_Projects_IsDeleted
  * IX_Projects_Name (for search)
- Query filter: !IsDeleted

**TaskItemConfiguration:**
- Table name: "TaskItems"
- Primary key: Id (auto-generated)
- Properties:
  * Title: required, max length 200
  * Description: optional, max length 2000
  * Priority: enum to string, max length 50
  * Status: enum to string, max length 50
  * DueDate: optional
  * AssigneeId: optional, index
  * ProjectId: required, foreign key, index
  * EstimatedHours: optional, precision (5,2)
  * CreatedAt: required
  * UpdatedAt: optional
  * IsDeleted: required, default false
  * RowVersion: shadow property, concurrency token
- Relationships:
  * HasOne Project with HasForeignKey(t => t.ProjectId)
  * OnDelete: Restrict
- Navigation:
  * Reference to Project aggregate root: public Project Project { get; set; } = null!;
- Indexes:
  * IX_TaskItems_ProjectId
  * IX_TaskItems_AssigneeId
  * IX_TaskItems_Status
  * IX_TaskItems_DueDate
  * IX_TaskItems_IsDeleted
- Query filter: !IsDeleted

**TeamConfiguration:**
- Table name: "Teams"
- Primary key: Id
- Properties:
  * Name: required, max length 100, unique index
  * Description: optional, max length 1000
  * LeaderId: required, index
  * CreatedAt: required
  * UpdatedAt: optional
  * IsDeleted: required, default false
  * RowVersion: shadow property, concurrency token
- Relationships:
  * HasMany TeamMembers with HasForeignKey(tm => tm.TeamId)
  * OnDelete: Restrict
- Indexes:
  * IX_Teams_Name (unique)
  * IX_Teams_LeaderId
  * IX_Teams_IsDeleted
- Query filter: !IsDeleted

**TeamMemberConfiguration:**
- Table name: "TeamMembers"
- Primary key: Id
- Properties:
  * UserId: required, index
  * Role: enum to string, max length 50
  * JoinedAt: required
  * TeamId: required, foreign key
  * CreatedAt: required
  * UpdatedAt: optional
  * IsDeleted: required, default false
  * RowVersion: shadow property, concurrency token
- Relationships:
  * HasOne Team with HasForeignKey(tm => tm.TeamId)
  * OnDelete: Restrict
- Navigation:
  * Reference to Team aggregate root: public Team Team { get; set; } = null!;
- Indexes:
  * IX_TeamMembers_TeamId
  * IX_TeamMembers_UserId
  * IX_TeamMembers_TeamId_UserId (unique composite - one user per team)
  * IX_TeamMembers_IsDeleted
- Query filter: !IsDeleted

**Value Object Conversions:**

For Money value object:
```csharp
builder.OwnsOne(e => e.Budget, money =>
{
    money.Property(m => m.Amount)
        .HasColumnName("BudgetAmount")
        .HasPrecision(18, 2)
        .IsRequired();
    
    money.Property(m => m.Currency)
        .HasColumnName("BudgetCurrency")
        .HasConversion<string>()
        .HasMaxLength(3)
        .IsRequired();
});
```

For DateRange value object:
```csharp
builder.OwnsOne(e => e.Timeline, range =>
{
    range.Property(r => r.StartDate)
        .HasColumnName("TimelineStartDate")
        .IsRequired();
    
    range.Property(r => r.EndDate)
        .HasColumnName("TimelineEndDate")
        .IsRequired();
});
```

For Email value object:
```csharp
builder.Property(e => e.ContactEmail)
    .HasConversion(
        email => email.Value,
        value => new Email(value))
    .HasMaxLength(255);
```

========================================
INTERCEPTORS
========================================

**AuditableEntityInterceptor:**

Implements: SaveChangesInterceptor

Purpose: Automatically set CreatedAt and UpdatedAt

Override SavingChangesAsync:
- Get all Added entities inheriting from Entity<TId>
- Set CreatedAt = DateTimeOffset.UtcNow
- Get all Modified entities inheriting from Entity<TId>
- Set UpdatedAt = DateTimeOffset.UtcNow
- Continue with base.SavingChangesAsync

**SoftDeleteInterceptor:**

Implements: SaveChangesInterceptor

Purpose: Convert hard deletes to soft deletes

Override SavingChangesAsync:
- Get all Deleted entities inheriting from Entity<TId>
- Change state to Modified
- Set IsDeleted = true
- Set UpdatedAt = DateTimeOffset.UtcNow
- Continue with base.SavingChangesAsync

**DispatchDomainEventsInterceptor:**

Implements: SaveChangesInterceptor

Purpose: Dispatch domain events after successful save

Constructor:
- Inject IMediator

Override SavingChangesAsync:
- Get all entities inheriting from AggregateRoot<TId>
- Collect all domain events
- Clear domain events from aggregates
- Call base.SavingChangesAsync
- After successful save, publish each event via mediator.Publish()
- Use try-finally to ensure events are cleared even on exception

Important:
- Events must be published AFTER SaveChanges succeeds
- Events must be cleared from aggregates
- Handle exceptions gracefully

========================================
REPOSITORY IMPLEMENTATIONS
========================================

**Base Repository Pattern:**

Both ProjectRepository and TeamRepository follow same pattern.

**ProjectRepository:**

Implements: IProjectRepository

Constructor:
- Inject ApplicationDbContext

Methods:
```csharp
public async Task AddAsync(Project entity, CancellationToken cancellationToken)
{
    await _context.Projects.AddAsync(entity, cancellationToken);
}

public void Update(Project entity)
{
    _context.Projects.Update(entity);
}

public void Remove(Project entity)
{
    _context.Projects.Remove(entity); // Soft delete interceptor handles this
}

public async Task<Project?> GetByIdAsync(int id, CancellationToken cancellationToken)
{
    return await _context.Projects
        .Include(p => p.Tasks) // Include child entities within aggregate
        .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
}

public async Task<IEnumerable<Project>> GetAllAsync(CancellationToken cancellationToken)
{
    return await _context.Projects
        .Include(p => p.Tasks)
        .ToListAsync(cancellationToken);
}

public async Task<IEnumerable<Project>> WhereAsync(
    Expression<Func<Project, bool>> predicate, 
    CancellationToken cancellationToken)
{
    return await _context.Projects
        .Include(p => p.Tasks)
        .Where(predicate)
        .ToListAsync(cancellationToken);
}
```

**TeamRepository:**

Same pattern as ProjectRepository but for Team aggregate:
- Include TeamMembers in GetByIdAsync and WhereAsync
- Use _context.Teams

========================================
UNIT OF WORK IMPLEMENTATION
========================================

**UnitOfWork:**

Implements: IUnitOfWork

Constructor:
- Inject ApplicationDbContext

Methods:
```csharp
public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
{
    return await _context.SaveChangesAsync(cancellationToken);
}

public async Task BeginTransactionAsync(CancellationToken cancellationToken)
{
    await _context.Database.BeginTransactionAsync(cancellationToken);
}

public async Task CommitTransactionAsync(CancellationToken cancellationToken)
{
    await _context.Database.CommitTransactionAsync(cancellationToken);
}

public async Task RollbackTransactionAsync(CancellationToken cancellationToken)
{
    await _context.Database.RollbackTransactionAsync(cancellationToken);
}
```

Note: Transaction methods use EF Core's Database.BeginTransactionAsync

========================================
SERVICE IMPLEMENTATIONS
========================================

**DateTimeProvider:**

Implements: IDateTimeProvider
```csharp
public class DateTimeProvider : IDateTimeProvider
{
    public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
}
```

**CurrentUserService:**

Implements: ICurrentUserService

Constructor:
- Inject IHttpContextAccessor

Properties:
```csharp
public int? UserId => GetUserIdFromClaims();
public string? UserName => _httpContextAccessor.HttpContext?.User?.Identity?.Name;
public bool IsAuthenticated => _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;

private int? GetUserIdFromClaims()
{
    var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    return int.TryParse(userIdClaim, out var userId) ? userId : null;
}
```

**EmailService:**

Implements: IEmailService

Constructor:
- Inject IAmazonSimpleEmailService (AWS SES client)
- Inject IConfiguration for sender email

Methods:
```csharp
public async Task SendEmailAsync(string to, string subject, string body, CancellationToken cancellationToken)
{
    var sendRequest = new SendEmailRequest
    {
        Source = _senderEmail,
        Destination = new Destination { ToAddresses = new List<string> { to } },
        Message = new Message
        {
            Subject = new Content(subject),
            Body = new Body { Html = new Content(body) }
        }
    };
    
    await _sesClient.SendEmailAsync(sendRequest, cancellationToken);
}
```

**FileStorageService:**

Interface: IFileStorageService (defined in Application/Common/Interfaces)

Implements file upload/download using AWS S3

Constructor:
- Inject IAmazonS3
- Inject IConfiguration for bucket name

Methods:
```csharp
public async Task<string> UploadFileAsync(Stream fileStream, string fileName, string contentType, CancellationToken cancellationToken)
{
    var key = $"{Guid.NewGuid()}/{fileName}";
    
    var request = new PutObjectRequest
    {
        BucketName = _bucketName,
        Key = key,
        InputStream = fileStream,
        ContentType = contentType
    };
    
    await _s3Client.PutObjectAsync(request, cancellationToken);
    
    return key; // Return S3 key as file identifier
}

public async Task<Stream> DownloadFileAsync(string fileKey, CancellationToken cancellationToken)
{
    var request = new GetObjectRequest
    {
        BucketName = _bucketName,
        Key = fileKey
    };
    
    var response = await _s3Client.GetObjectAsync(request, cancellationToken);
    return response.ResponseStream;
}

public async Task DeleteFileAsync(string fileKey, CancellationToken cancellationToken)
{
    var request = new DeleteObjectRequest
    {
        BucketName = _bucketName,
        Key = fileKey
    };
    
    await _s3Client.DeleteObjectAsync(request, cancellationToken);
}
```

========================================
AWS SERVICE IMPLEMENTATIONS
========================================

**S3Service:**

Wrapper around AWS S3 client for dependency injection.

**SESService:**

Wrapper around AWS SES client for dependency injection.

**SecretsManagerService:**

Implements: ISecretsManagerService

Methods:
```csharp
public async Task<string> GetSecretAsync(string secretName, CancellationToken cancellationToken)
{
    var request = new GetSecretValueRequest
    {
        SecretId = secretName
    };
    
    var response = await _secretsManagerClient.GetSecretValueAsync(request, cancellationToken);
    return response.SecretString;
}
```

========================================
DATABASE INITIALIZER
========================================

**ApplicationDbContextInitializer:**

Purpose: Handle database migration and seeding

Constructor:
- Inject ApplicationDbContext
- Inject ILogger<ApplicationDbContextInitializer>

Methods:
```csharp
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
    // Check if database is empty
    if (!await _context.Projects.AnyAsync())
    {
        // Seed sample data for development
        var project = Project.Create(
            "Sample Project",
            "This is a sample project for development",
            1); // OwnerId = 1
        
        await _context.Projects.AddAsync(project);
        await _context.SaveChangesAsync();
    }
}
```

========================================
IDENTITY (OPTIONAL)
========================================

**ApplicationUser:**

If using ASP.NET Core Identity:
```csharp
public class ApplicationUser : IdentityUser<int>
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTimeOffset CreatedAt { get; set; }
}
```

**IdentityService:**

Implements: IIdentityService (defined in Application)

Methods for user management, authentication, authorization.

Note: This is optional based on authentication requirements.

========================================
DEPENDENCY INJECTION
========================================

**DependencyInjection.cs:**

Static class with extension method.
```csharp
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // DbContext with PostgreSQL
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(
                configuration.GetConnectionString("DefaultConnection"),
                npgsqlOptions =>
                {
                    npgsqlOptions.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
                    npgsqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 3,
                        maxRetryDelay: TimeSpan.FromSeconds(30),
                        errorCodesToAdd: null);
                });
            
            options.EnableSensitiveDataLogging(
                configuration.GetValue<bool>("DatabaseSettings:EnableSensitiveDataLogging"));
        });
        
        // Interceptors
        services.AddSingleton<AuditableEntityInterceptor>();
        services.AddSingleton<SoftDeleteInterceptor>();
        services.AddScoped<DispatchDomainEventsInterceptor>();
        
        // Register interceptors with DbContext
        services.AddScoped<ApplicationDbContext>(provider =>
        {
            var options = provider.GetRequiredService<DbContextOptions<ApplicationDbContext>>();
            var mediator = provider.GetRequiredService<IMediator>();
            var auditInterceptor = provider.GetRequiredService<AuditableEntityInterceptor>();
            var softDeleteInterceptor = provider.GetRequiredService<SoftDeleteInterceptor>();
            var domainEventInterceptor = provider.GetRequiredService<DispatchDomainEventsInterceptor>();
            
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>(options);
            optionsBuilder.AddInterceptors(auditInterceptor, softDeleteInterceptor, domainEventInterceptor);
            
            return new ApplicationDbContext(optionsBuilder.Options, mediator);
        });
        
        // Repositories
        services.AddScoped<IProjectRepository, ProjectRepository>();
        services.AddScoped<ITeamRepository, TeamRepository>();
        
        // Unit of Work
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        
        // Application Services
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        services.AddTransient<IEmailService, EmailService>();
        services.AddTransient<IFileStorageService, FileStorageService>();
        
        // AWS Services
        services.AddAWSService<IAmazonS3>();
        services.AddAWSService<IAmazonSimpleEmailService>();
        services.AddAWSService<IAmazonSecretsManager>();
        
        // HttpContextAccessor for CurrentUserService
        services.AddHttpContextAccessor();
        
        // Database Initializer
        services.AddScoped<ApplicationDbContextInitializer>();
        
        return services;
    }
}
```

========================================
CONFIGURATION REQUIREMENTS
========================================

**appsettings.json structure expected:**
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=TaskManagement;Username=postgres;Password=postgres"
  },
  "DatabaseSettings": {
    "EnableSensitiveDataLogging": false
  },
  "AWS": {
    "Region": "us-east-1",
    "S3": {
      "BucketName": "task-management-files"
    },
    "SES": {
      "SenderEmail": "noreply@taskmanagement.com"
    }
  }
}
```

========================================
MIGRATION REQUIREMENTS
========================================

Initial migration should include:
- All entity tables (Projects, TaskItems, Teams, TeamMembers)
- All indexes defined in configurations
- All foreign key constraints
- Query filters applied
- Enum to string conversions
- Value object column mappings

Migration naming:
- Format: 20250221120000_InitialCreate
- Use: dotnet ef migrations add InitialCreate

Migration application:
- Automatic via ApplicationDbContextInitializer.InitializeAsync()
- Or manual via: dotnet ef database update

========================================
IMPLEMENTATION CONSTRAINTS
========================================

- All database access must go through DbContext
- No direct SQL unless absolutely necessary (use EF Core features)
- All entities must have configurations in separate classes
- No Data Annotations in Domain (use Fluent API only)
- Use AsNoTracking() for read-only queries when appropriate
- Implement retry logic for transient failures
- Use compiled queries for frequently executed queries
- Connection pooling enabled by default
- Transaction isolation level: Read Committed (default)
- Optimize for PostgreSQL-specific features
- Use JSONB for complex value objects if needed
- Use PostgreSQL arrays for collections if needed
- Proper index strategy for performance
- Monitor and log slow queries
- Use projection (Select) to minimize data transfer
- Avoid N+1 query problems (use Include/ThenInclude properly)
- Use split queries for multiple collections if needed
- Implement proper connection string security (use Secrets Manager in production)

========================================
EXPECTED OUTPUT FILES
========================================

Total: ~25 files

**Persistence (12 files):**
1. src/Infrastructure/Persistence/ApplicationDbContext.cs
2. src/Infrastructure/Persistence/ApplicationDbContextInitializer.cs
3. src/Infrastructure/Persistence/UnitOfWork.cs
4. src/Infrastructure/Persistence/Configurations/ProjectConfiguration.cs
5. src/Infrastructure/Persistence/Configurations/TaskItemConfiguration.cs
6. src/Infrastructure/Persistence/Configurations/TeamConfiguration.cs
7. src/Infrastructure/Persistence/Configurations/TeamMemberConfiguration.cs
8. src/Infrastructure/Persistence/Interceptors/AuditableEntityInterceptor.cs
9. src/Infrastructure/Persistence/Interceptors/SoftDeleteInterceptor.cs
10. src/Infrastructure/Persistence/Interceptors/DispatchDomainEventsInterceptor.cs
11. src/Infrastructure/Persistence/Repositories/ProjectRepository.cs
12. src/Infrastructure/Persistence/Repositories/TeamRepository.cs

**Services (4 files):**
13. src/Infrastructure/Services/DateTimeProvider.cs
14. src/Infrastructure/Services/CurrentUserService.cs
15. src/Infrastructure/Services/EmailService.cs
16. src/Infrastructure/Services/FileStorageService.cs

**AWS (3 files):**
17. src/Infrastructure/AWS/S3Service.cs
18. src/Infrastructure/AWS/SESService.cs
19. src/Infrastructure/AWS/SecretsManagerService.cs

**Identity (2 files - optional):**
20. src/Infrastructure/Identity/ApplicationUser.cs
21. src/Infrastructure/Identity/IdentityService.cs

**Root (1 file):**
22. src/Infrastructure/DependencyInjection.cs

**Common/Interfaces (if not in Application):**
23. src/Infrastructure/Common/IFileStorageService.cs (or in Application/Common/Interfaces)

========================================
OUTPUT FORMAT
========================================

Return full C# code for all files.

Each file must be separated with:

// File: src/Infrastructure/...

Use proper namespaces following the folder structure pattern:
- TaskManagement.Infrastructure.Persistence
- TaskManagement.Infrastructure.Services
- TaskManagement.Infrastructure.AWS
- TaskManagement.Infrastructure.Identity

Include all necessary using statements.
Reference Domain and Application namespaces where needed.

Only return code. No explanations. No markdown. No comments outside code.

Begin output now.