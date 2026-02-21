# Application Layer - Clean Architecture Prompt

You are a senior .NET software architect.

Generate a complete Application/Business Layer using:

- Clean Architecture
- Domain-Driven Design (DDD)
- CQRS Pattern
- MediatR
- FluentValidation
- AutoMapper
- .NET 10
- C#

The output must ONLY contain the Application Layer code.
No explanations. No comments outside code.
Follow the folder structure and rules exactly.

========================================
BUSINESS DOMAIN CONTEXT
========================================

This is a Task Management System with the following use cases:

**Project Management:**
- Create Project (Command)
- Archive Project (Command)
- Get Project by Id (Query)
- Get All Projects (Query)
- Get Projects by Owner (Query)
- Update Project Details (Command)

**Task Management:**
- Create Task in Project (Command)
- Assign Task to User (Command)
- Update Task Status (Command)
- Complete Task (Command)
- Update Task Details (Command)
- Get Task by Id (Query)
- Get Tasks by Project (Query)
- Get Tasks by Assignee (Query)
- Get Overdue Tasks (Query)

**Team Management:**
- Create Team (Command)
- Add Member to Team (Command)
- Remove Member from Team (Command)
- Change Team Leader (Command)
- Get Team by Id (Query)
- Get All Teams (Query)
- Get Teams by Leader (Query)

**Cross-Cutting:**
- All commands return Result<T> (success/failure pattern)
- All queries return data DTOs
- Validation before command execution
- Domain events published after successful commands
- Transaction management via UnitOfWork

========================================
ARCHITECTURE RULES
========================================

- Application layer must be inside: src/Application
- Depends on Domain layer ONLY (no Infrastructure dependencies)
- Use MediatR for CQRS (IRequest, IRequestHandler)
- Use FluentValidation for input validation
- Use AutoMapper for Domain -> DTO mapping
- All handlers must be async with CancellationToken
- Commands return Result<T> or Result (custom result wrapper)
- Queries return DTOs directly
- Validators must be separate classes (one per command/query)
- Use IPipelineBehavior for cross-cutting concerns
- Domain events handled via INotificationHandler
- No direct database access (use repositories from Domain)
- No EF Core references
- Transaction management via IUnitOfWork
- DateTimeOffset for all dates
- Enum validation in validators
- Null reference handling
- Pagination for list queries
- Sorting and filtering support

========================================
FOLDER STRUCTURE
========================================

src
 └── Application
       ├── Common
       │     ├── Interfaces
       │     │     ├── ICurrentUserService.cs
       │     │     ├── IDateTimeProvider.cs
       │     │     └── IEmailService.cs
       │     ├── Models
       │     │     ├── Result.cs
       │     │     ├── Result{T}.cs
       │     │     ├── PaginatedList{T}.cs
       │     │     └── Error.cs
       │     ├── Behaviors
       │     │     ├── ValidationBehavior.cs
       │     │     ├── LoggingBehavior.cs
       │     │     └── TransactionBehavior.cs
       │     ├── Exceptions
       │     │     ├── ValidationException.cs
       │     │     ├── NotFoundException.cs
       │     │     └── ForbiddenAccessException.cs
       │     └── Mappings
       │           └── MappingProfile.cs
       │
       ├── Projects
       │     ├── Commands
       │     │     ├── CreateProject
       │     │     │     ├── CreateProjectCommand.cs
       │     │     │     ├── CreateProjectCommandHandler.cs
       │     │     │     └── CreateProjectCommandValidator.cs
       │     │     ├── ArchiveProject
       │     │     │     ├── ArchiveProjectCommand.cs
       │     │     │     ├── ArchiveProjectCommandHandler.cs
       │     │     │     └── ArchiveProjectCommandValidator.cs
       │     │     └── UpdateProject
       │     │           ├── UpdateProjectCommand.cs
       │     │           ├── UpdateProjectCommandHandler.cs
       │     │           └── UpdateProjectCommandValidator.cs
       │     ├── Queries
       │     │     ├── GetProjectById
       │     │     │     ├── GetProjectByIdQuery.cs
       │     │     │     ├── GetProjectByIdQueryHandler.cs
       │     │     │     └── GetProjectByIdQueryValidator.cs
       │     │     ├── GetAllProjects
       │     │     │     ├── GetAllProjectsQuery.cs
       │     │     │     └── GetAllProjectsQueryHandler.cs
       │     │     └── GetProjectsByOwner
       │     │           ├── GetProjectsByOwnerQuery.cs
       │     │           ├── GetProjectsByOwnerQueryHandler.cs
       │     │           └── GetProjectsByOwnerQueryValidator.cs
       │     ├── DTOs
       │     │     ├── ProjectDto.cs
       │     │     ├── ProjectDetailDto.cs
       │     │     └── ProjectListDto.cs
       │     └── EventHandlers
       │           ├── ProjectCreatedEventHandler.cs
       │           └── ProjectArchivedEventHandler.cs
       │
       ├── Tasks
       │     ├── Commands
       │     │     ├── CreateTask
       │     │     │     ├── CreateTaskCommand.cs
       │     │     │     ├── CreateTaskCommandHandler.cs
       │     │     │     └── CreateTaskCommandValidator.cs
       │     │     ├── AssignTask
       │     │     │     ├── AssignTaskCommand.cs
       │     │     │     ├── AssignTaskCommandHandler.cs
       │     │     │     └── AssignTaskCommandValidator.cs
       │     │     ├── UpdateTaskStatus
       │     │     │     ├── UpdateTaskStatusCommand.cs
       │     │     │     ├── UpdateTaskStatusCommandHandler.cs
       │     │     │     └── UpdateTaskStatusCommandValidator.cs
       │     │     ├── CompleteTask
       │     │     │     ├── CompleteTaskCommand.cs
       │     │     │     ├── CompleteTaskCommandHandler.cs
       │     │     │     └── CompleteTaskCommandValidator.cs
       │     │     └── UpdateTask
       │     │           ├── UpdateTaskCommand.cs
       │     │           ├── UpdateTaskCommandHandler.cs
       │     │           └── UpdateTaskCommandValidator.cs
       │     ├── Queries
       │     │     ├── GetTaskById
       │     │     │     ├── GetTaskByIdQuery.cs
       │     │     │     ├── GetTaskByIdQueryHandler.cs
       │     │     │     └── GetTaskByIdQueryValidator.cs
       │     │     ├── GetTasksByProject
       │     │     │     ├── GetTasksByProjectQuery.cs
       │     │     │     ├── GetTasksByProjectQueryHandler.cs
       │     │     │     └── GetTasksByProjectQueryValidator.cs
       │     │     ├── GetTasksByAssignee
       │     │     │     ├── GetTasksByAssigneeQuery.cs
       │     │     │     ├── GetTasksByAssigneeQueryHandler.cs
       │     │     │     └── GetTasksByAssigneeQueryValidator.cs
       │     │     └── GetOverdueTasks
       │     │           ├── GetOverdueTasksQuery.cs
       │     │           └── GetOverdueTasksQueryHandler.cs
       │     ├── DTOs
       │     │     ├── TaskDto.cs
       │     │     ├── TaskDetailDto.cs
       │     │     └── TaskListDto.cs
       │     └── EventHandlers
       │           ├── TaskCreatedEventHandler.cs
       │           ├── TaskAssignedEventHandler.cs
       │           └── TaskCompletedEventHandler.cs
       │
       ├── Teams
       │     ├── Commands
       │     │     ├── CreateTeam
       │     │     │     ├── CreateTeamCommand.cs
       │     │     │     ├── CreateTeamCommandHandler.cs
       │     │     │     └── CreateTeamCommandValidator.cs
       │     │     ├── AddTeamMember
       │     │     │     ├── AddTeamMemberCommand.cs
       │     │     │     ├── AddTeamMemberCommandHandler.cs
       │     │     │     └── AddTeamMemberCommandValidator.cs
       │     │     ├── RemoveTeamMember
       │     │     │     ├── RemoveTeamMemberCommand.cs
       │     │     │     ├── RemoveTeamMemberCommandHandler.cs
       │     │     │     └── RemoveTeamMemberCommandValidator.cs
       │     │     └── ChangeTeamLeader
       │     │           ├── ChangeTeamLeaderCommand.cs
       │     │           ├── ChangeTeamLeaderCommandHandler.cs
       │     │           └── ChangeTeamLeaderCommandValidator.cs
       │     ├── Queries
       │     │     ├── GetTeamById
       │     │     │     ├── GetTeamByIdQuery.cs
       │     │     │     ├── GetTeamByIdQueryHandler.cs
       │     │     │     └── GetTeamByIdQueryValidator.cs
       │     │     ├── GetAllTeams
       │     │     │     ├── GetAllTeamsQuery.cs
       │     │     │     └── GetAllTeamsQueryHandler.cs
       │     │     └── GetTeamsByLeader
       │     │           ├── GetTeamsByLeaderQuery.cs
       │     │           ├── GetTeamsByLeaderQueryHandler.cs
       │     │           └── GetTeamsByLeaderQueryValidator.cs
       │     ├── DTOs
       │     │     ├── TeamDto.cs
       │     │     ├── TeamDetailDto.cs
       │     │     ├── TeamListDto.cs
       │     │     └── TeamMemberDto.cs
       │     └── EventHandlers
       │           ├── TeamMemberAddedEventHandler.cs
       │           └── TeamLeaderChangedEventHandler.cs
       │
       └── DependencyInjection.cs

========================================
PACKAGE REFERENCES
========================================

Required NuGet packages (referenced in Application.csproj):

- MediatR (13.x or later)
- FluentValidation (11.x or later)
- FluentValidation.DependencyInjectionExtensions
- AutoMapper (13.x or later)
- AutoMapper.Extensions.Microsoft.DependencyInjection

========================================
COMMON COMPONENTS REQUIREMENTS
========================================

----------------------------------------
Result Pattern
----------------------------------------

**Result class (for commands without return value):**
```csharp
public class Result
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public Error Error { get; }
    
    protected Result(bool isSuccess, Error error)
    {
        IsSuccess = isSuccess;
        Error = error;
    }
    
    public static Result Success() => new(true, Error.None);
    public static Result Failure(Error error) => new(false, error);
}
```

**Result<T> class (for commands with return value):**
```csharp
public class Result<T> : Result
{
    public T? Value { get; }
    
    private Result(T? value, bool isSuccess, Error error) 
        : base(isSuccess, error)
    {
        Value = value;
    }
    
    public static Result<T> Success(T value) => new(value, true, Error.None);
    public static new Result<T> Failure(Error error) => new(default, false, error);
}
```

**Error record:**
```csharp
public record Error(string Code, string Message)
{
    public static readonly Error None = new(string.Empty, string.Empty);
}
```

----------------------------------------
PaginatedList<T>
----------------------------------------

Properties:
- List<T> Items
- int PageNumber
- int TotalPages
- int TotalCount
- bool HasPreviousPage
- bool HasNextPage

Static factory method:
- Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageNumber, int pageSize, CancellationToken cancellationToken)

----------------------------------------
ValidationBehavior<TRequest, TResponse>
----------------------------------------

Implements: IPipelineBehavior<TRequest, TResponse>

- Inject IEnumerable<IValidator<TRequest>>
- If no validators, continue pipeline
- If validators exist, validate request
- If validation fails, throw ValidationException with all errors
- ValidationException must contain Dictionary<string, string[]> Errors

----------------------------------------
LoggingBehavior<TRequest, TResponse>
----------------------------------------

Implements: IPipelineBehavior<TRequest, TResponse>

- Inject ILogger<LoggingBehavior<TRequest, TResponse>>
- Log request name and timestamp before execution
- Log response and duration after execution
- Use structured logging

----------------------------------------
TransactionBehavior<TRequest, TResponse>
----------------------------------------

Implements: IPipelineBehavior<TRequest, TResponse>

- Only apply to IRequest<Result> or IRequest<Result<T>>
- Inject IUnitOfWork
- Begin transaction before handler
- Commit on success
- Rollback on exception
- Ensure proper disposal

----------------------------------------
Exceptions
----------------------------------------

**ValidationException:**
- Inherits from Exception
- Constructor: ValidationException(IDictionary<string, string[]> errors)
- Property: IDictionary<string, string[]> Errors

**NotFoundException:**
- Inherits from Exception
- Constructor: NotFoundException(string name, object key)
- Message: $"Entity '{name}' ({key}) was not found."

**ForbiddenAccessException:**
- Inherits from Exception
- Constructor: ForbiddenAccessException()
- Message: "Access denied."

----------------------------------------
ICurrentUserService
----------------------------------------

Properties:
- int? UserId { get; }
- string? UserName { get; }
- bool IsAuthenticated { get; }

Implemented in Infrastructure layer.

----------------------------------------
IDateTimeProvider
----------------------------------------

Methods:
- DateTimeOffset UtcNow { get; }

Implemented in Infrastructure layer.
Used for testable time in commands.

----------------------------------------
IEmailService
----------------------------------------

Methods:
- Task SendEmailAsync(string to, string subject, string body, CancellationToken cancellationToken);

Implemented in Infrastructure layer.
Used in event handlers for notifications.

========================================
CQRS PATTERN REQUIREMENTS
========================================

**Commands:**
- Implement IRequest<Result> or IRequest<Result<T>>
- Must have corresponding CommandHandler implementing IRequestHandler
- Must have corresponding CommandValidator inheriting AbstractValidator
- Handler must:
  * Inject required repositories via constructor
  * Inject IUnitOfWork
  * Validate business rules
  * Call aggregate methods
  * Call repository methods
  * Call await _unitOfWork.SaveChangesAsync(cancellationToken)
  * Return Result.Success() or Result.Failure()
- Named with verb: CreateProjectCommand, ArchiveProjectCommand

**Queries:**
- Implement IRequest<TDto> or IRequest<PaginatedList<TDto>>
- Must have corresponding QueryHandler implementing IRequestHandler
- May have QueryValidator if parameters need validation
- Handler must:
  * Inject required repositories via constructor
  * Use repository methods or direct IQueryable when needed
  * Map domain entities to DTOs using AutoMapper
  * Return DTOs directly (not Result pattern)
- Named with Get prefix: GetProjectByIdQuery, GetAllProjectsQuery

**Validators:**
- Inherit from AbstractValidator<TCommand> or AbstractValidator<TQuery>
- Define rules in constructor
- Use RuleFor() with built-in and custom validators
- Common rules:
  * NotEmpty() for required strings
  * MaximumLength() for string length
  * GreaterThan() / LessThan() for numbers
  * Must() for custom business rules
  * IsInEnum() for enum validation
  * EmailAddress() for email format

**DTOs:**
- Plain POCOs with get/init properties
- No business logic
- Used for query responses only
- Named with Dto suffix: ProjectDto, TaskDetailDto
- May contain nested DTOs
- AutoMapper maps from Domain entities to DTOs

**Event Handlers:**
- Implement INotificationHandler<TDomainEvent>
- Async with CancellationToken
- Handle side effects (emails, notifications, logging)
- Do NOT modify aggregates
- Multiple handlers can handle same event
- Named with Event name + Handler: ProjectCreatedEventHandler

========================================
AUTOMAPPER REQUIREMENTS
========================================

**MappingProfile:**
- Inherits from Profile
- Configure all Domain Entity -> DTO mappings in constructor
- Use CreateMap<TSource, TDestination>()
- Use ForMember() for custom mappings
- Use MapFrom() for complex property mappings
- Example structure:
```csharp
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Project, ProjectDto>();
        CreateMap<Project, ProjectDetailDto>()
            .ForMember(d => d.TaskCount, opt => opt.MapFrom(s => s.Tasks.Count));
        CreateMap<TaskItem, TaskDto>();
        // ... more mappings
    }
}
```

========================================
DEPENDENCY INJECTION REQUIREMENTS
========================================

**DependencyInjection.cs:**
- Static class with static method: AddApplication(this IServiceCollection services)
- Register MediatR from current assembly
- Register FluentValidation validators from current assembly
- Register AutoMapper profiles from current assembly
- Register pipeline behaviors in order:
  1. LoggingBehavior
  2. ValidationBehavior
  3. TransactionBehavior
- Return IServiceCollection for chaining

Example structure:
```csharp
public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg => 
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionBehavior<,>));
        
        return services;
    }
}
```

========================================
IMPLEMENTATION CONSTRAINTS
========================================

- All handlers must be async with CancellationToken parameter
- Use CancellationToken.None default only in examples
- Commands modify state and use Result pattern
- Queries read state and return DTOs directly
- No business logic in DTOs
- No repository calls in event handlers
- Validators must not make database calls
- Use ICurrentUserService for authorization checks
- Use IDateTimeProvider instead of DateTimeOffset.UtcNow in handlers
- Proper null checking for GetByIdAsync results
- Throw NotFoundException when entity not found
- Throw ForbiddenAccessException for authorization failures
- All validation messages must be descriptive
- Use const for validation error messages
- Pagination defaults: PageNumber = 1, PageSize = 10
- Maximum page size: 100

========================================
SAMPLE COMMAND STRUCTURE
========================================

**CreateProjectCommand.cs:**
```csharp
public record CreateProjectCommand(
    string Name,
    string? Description,
    int OwnerId
) : IRequest<Result<int>>;
```

**CreateProjectCommandValidator.cs:**
```csharp
public class CreateProjectCommandValidator : AbstractValidator<CreateProjectCommand>
{
    public CreateProjectCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Project name is required.")
            .MaximumLength(200).WithMessage("Project name must not exceed 200 characters.");
        
        RuleFor(x => x.Description)
            .MaximumLength(2000).WithMessage("Description must not exceed 2000 characters.")
            .When(x => !string.IsNullOrEmpty(x.Description));
        
        RuleFor(x => x.OwnerId)
            .GreaterThan(0).WithMessage("Owner ID must be greater than 0.");
    }
}
```

**CreateProjectCommandHandler.cs:**
```csharp
public class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, Result<int>>
{
    private readonly IProjectRepository _projectRepository;
    private readonly IUnitOfWork _unitOfWork;
    
    public CreateProjectCommandHandler(
        IProjectRepository projectRepository,
        IUnitOfWork unitOfWork)
    {
        _projectRepository = projectRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Result<int>> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
    {
        var project = Project.Create(
            request.Name,
            request.Description,
            request.OwnerId);
        
        await _projectRepository.AddAsync(project, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Result<int>.Success(project.Id);
    }
}
```

========================================
SAMPLE QUERY STRUCTURE
========================================

**GetProjectByIdQuery.cs:**
```csharp
public record GetProjectByIdQuery(int ProjectId) : IRequest<ProjectDetailDto>;
```

**GetProjectByIdQueryValidator.cs:**
```csharp
public class GetProjectByIdQueryValidator : AbstractValidator<GetProjectByIdQuery>
{
    public GetProjectByIdQueryValidator()
    {
        RuleFor(x => x.ProjectId)
            .GreaterThan(0).WithMessage("Project ID must be greater than 0.");
    }
}
```

**GetProjectByIdQueryHandler.cs:**
```csharp
public class GetProjectByIdQueryHandler : IRequestHandler<GetProjectByIdQuery, ProjectDetailDto>
{
    private readonly IProjectRepository _projectRepository;
    private readonly IMapper _mapper;
    
    public GetProjectByIdQueryHandler(
        IProjectRepository projectRepository,
        IMapper mapper)
    {
        _projectRepository = projectRepository;
        _mapper = mapper;
    }
    
    public async Task<ProjectDetailDto> Handle(GetProjectByIdQuery request, CancellationToken cancellationToken)
    {
        var project = await _projectRepository.GetByIdAsync(request.ProjectId, cancellationToken);
        
        if (project == null)
        {
            throw new NotFoundException(nameof(Project), request.ProjectId);
        }
        
        return _mapper.Map<ProjectDetailDto>(project);
    }
}
```

========================================
DTO STRUCTURE GUIDELINES
========================================

**Simple DTO (for lists):**
```csharp
public record ProjectDto
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public ProjectStatus Status { get; init; }
    public int OwnerId { get; init; }
    public DateTimeOffset CreatedAt { get; init; }
}
```

**Detail DTO (for single item with relations):**
```csharp
public record ProjectDetailDto
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
    public ProjectStatus Status { get; init; }
    public int OwnerId { get; init; }
    public int TaskCount { get; init; }
    public IReadOnlyCollection<TaskDto> Tasks { get; init; } = Array.Empty<TaskDto>();
    public DateTimeOffset CreatedAt { get; init; }
    public DateTimeOffset? UpdatedAt { get; init; }
}
```

========================================
EXPECTED OUTPUT FILES
========================================

Total: ~100 files

**Common (15 files):**
- Interfaces (3)
- Models (4)
- Behaviors (3)
- Exceptions (3)
- Mappings (1)
- DependencyInjection (1)

**Projects (~20 files):**
- Commands (9: 3 commands × 3 files each)
- Queries (9: 3 queries, some with validators)
- DTOs (3)
- EventHandlers (2)

**Tasks (~30 files):**
- Commands (15: 5 commands × 3 files each)
- Queries (12: 4 queries, some with validators)
- DTOs (3)
- EventHandlers (3)

**Teams (~20 files):**
- Commands (12: 4 commands × 3 files each)
- Queries (9: 3 queries, some with validators)
- DTOs (4)
- EventHandlers (2)

========================================
OUTPUT FORMAT
========================================

Return full C# code for all files.

Each file must be separated with:

// File: src/Application/...

Use proper namespaces following the folder structure pattern:
- TaskManagement.Application.Common.*
- TaskManagement.Application.Projects.*
- TaskManagement.Application.Tasks.*
- TaskManagement.Application.Teams.*

Include all necessary using statements.

Only return code. No explanations. No markdown. No comments outside code.

Begin output now.