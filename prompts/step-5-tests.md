# Tests Layer - Clean Architecture Prompt

You are a senior .NET software architect.

Generate comprehensive Test suites using:

- xUnit
- FluentAssertions
- Moq
- AutoFixture
- Bogus (for realistic test data)
- Testcontainers (for integration tests)
- Respawn (for database cleanup)
- WebApplicationFactory (for API tests)
- .NET 10
- C#

The output must contain BOTH Unit Tests and Integration Tests.
No explanations. No comments outside code.
Follow the folder structure and rules exactly.

========================================
TESTING SCOPE
========================================

**Unit Tests:**
- Domain Layer (entities, aggregates, value objects, domain events)
- Application Layer (handlers, validators, behaviors, mappings)

**Integration Tests:**
- Infrastructure Layer (DbContext, repositories, interceptors, configurations)
- API Layer (controllers, endpoints, middleware, authentication, full workflows)

========================================
ARCHITECTURE RULES
========================================

**Unit Tests:**
- Test projects inside: tests/
- Naming: Domain.UnitTests, Application.UnitTests
- One test class per production class
- Test method naming: MethodName_Scenario_ExpectedResult
- Arrange-Act-Assert (AAA) pattern
- Use FluentAssertions for all assertions
- Mock external dependencies with Moq
- Use AutoFixture for test data generation
- Use Bogus for realistic domain data
- Each test tests ONE thing only
- Tests must be independent (no shared state)
- Tests must be deterministic
- Use [Theory] with [InlineData] for parameterized tests
- Use [Fact] for simple single-case tests
- Group related tests in nested classes

**Integration Tests:**
- Test projects inside: tests/
- Naming: Infrastructure.IntegrationTests, Api.IntegrationTests
- Use Testcontainers for real PostgreSQL database
- Use Respawn for database cleanup between tests
- Use WebApplicationFactory for API tests
- Dispose resources properly (IAsyncLifetime)
- Each test class has isolated database state
- Tests run in parallel when possible
- Test against actual infrastructure (no mocking)
- Verify database state after operations
- Test complete request/response cycle

========================================
FOLDER STRUCTURE
========================================

tests
 ├── Domain.UnitTests
 │     ├── Common
 │     │     ├── EntityTests.cs
 │     │     ├── AggregateRootTests.cs
 │     │     └── ValueObjectTests.cs
 │     ├── Aggregates
 │     │     ├── ProjectAggregate
 │     │     │     ├── ProjectTests.cs
 │     │     │     ├── TaskItemTests.cs
 │     │     │     └── ValueObjects
 │     │     │           └── DateRangeTests.cs
 │     │     └── TeamAggregate
 │     │           ├── TeamTests.cs
 │     │           ├── TeamMemberTests.cs
 │     │           └── ValueObjects
 │     │                 ├── MoneyTests.cs
 │     │                 └── EmailTests.cs
 │     ├── Builders
 │     │     ├── ProjectBuilder.cs
 │     │     ├── TaskItemBuilder.cs
 │     │     ├── TeamBuilder.cs
 │     │     └── TeamMemberBuilder.cs
 │     └── Domain.UnitTests.csproj
 │
 ├── Application.UnitTests
 │     ├── Common
 │     │     ├── Behaviors
 │     │     │     ├── ValidationBehaviorTests.cs
 │     │     │     ├── LoggingBehaviorTests.cs
 │     │     │     └── TransactionBehaviorTests.cs
 │     │     └── Mappings
 │     │           └── MappingProfileTests.cs
 │     ├── Projects
 │     │     ├── Commands
 │     │     │     ├── CreateProject
 │     │     │     │     ├── CreateProjectCommandHandlerTests.cs
 │     │     │     │     └── CreateProjectCommandValidatorTests.cs
 │     │     │     ├── ArchiveProject
 │     │     │     │     ├── ArchiveProjectCommandHandlerTests.cs
 │     │     │     │     └── ArchiveProjectCommandValidatorTests.cs
 │     │     │     └── UpdateProject
 │     │     │           ├── UpdateProjectCommandHandlerTests.cs
 │     │     │           └── UpdateProjectCommandValidatorTests.cs
 │     │     ├── Queries
 │     │     │     ├── GetProjectById
 │     │     │     │     ├── GetProjectByIdQueryHandlerTests.cs
 │     │     │     │     └── GetProjectByIdQueryValidatorTests.cs
 │     │     │     ├── GetAllProjects
 │     │     │     │     └── GetAllProjectsQueryHandlerTests.cs
 │     │     │     └── GetProjectsByOwner
 │     │     │           ├── GetProjectsByOwnerQueryHandlerTests.cs
 │     │     │           └── GetProjectsByOwnerQueryValidatorTests.cs
 │     │     └── EventHandlers
 │     │           ├── ProjectCreatedEventHandlerTests.cs
 │     │           └── ProjectArchivedEventHandlerTests.cs
 │     ├── Tasks
 │     │     ├── Commands
 │     │     │     ├── CreateTask
 │     │     │     ├── AssignTask
 │     │     │     ├── UpdateTaskStatus
 │     │     │     ├── CompleteTask
 │     │     │     └── UpdateTask
 │     │     ├── Queries
 │     │     │     ├── GetTaskById
 │     │     │     ├── GetTasksByProject
 │     │     │     ├── GetTasksByAssignee
 │     │     │     └── GetOverdueTasks
 │     │     └── EventHandlers
 │     │           ├── TaskCreatedEventHandlerTests.cs
 │     │           ├── TaskAssignedEventHandlerTests.cs
 │     │           └── TaskCompletedEventHandlerTests.cs
 │     ├── Teams
 │     │     ├── Commands
 │     │     │     ├── CreateTeam
 │     │     │     ├── AddTeamMember
 │     │     │     ├── RemoveTeamMember
 │     │     │     └── ChangeTeamLeader
 │     │     ├── Queries
 │     │     │     ├── GetTeamById
 │     │     │     ├── GetAllTeams
 │     │     │     └── GetTeamsByLeader
 │     │     └── EventHandlers
 │     │           ├── TeamMemberAddedEventHandlerTests.cs
 │     │           └── TeamLeaderChangedEventHandlerTests.cs
 │     ├── Mocks
 │     │     ├── MockRepositoryFactory.cs
 │     │     ├── MockUnitOfWork.cs
 │     │     └── MockCurrentUserService.cs
 │     ├── Fixtures
 │     │     ├── ProjectFixture.cs
 │     │     ├── TaskFixture.cs
 │     │     └── TeamFixture.cs
 │     └── Application.UnitTests.csproj
 │
 ├── Infrastructure.IntegrationTests
 │     ├── Persistence
 │     │     ├── ApplicationDbContextTests.cs
 │     │     ├── Configurations
 │     │     │     ├── ProjectConfigurationTests.cs
 │     │     │     ├── TaskItemConfigurationTests.cs
 │     │     │     ├── TeamConfigurationTests.cs
 │     │     │     └── TeamMemberConfigurationTests.cs
 │     │     ├── Interceptors
 │     │     │     ├── AuditableEntityInterceptorTests.cs
 │     │     │     ├── SoftDeleteInterceptorTests.cs
 │     │     │     └── DispatchDomainEventsInterceptorTests.cs
 │     │     ├── Repositories
 │     │     │     ├── ProjectRepositoryTests.cs
 │     │     │     └── TeamRepositoryTests.cs
 │     │     └── UnitOfWorkTests.cs
 │     ├── Common
 │     │     ├── DatabaseFixture.cs
 │     │     ├── IntegrationTestBase.cs
 │     │     └── TestData.cs
 │     └── Infrastructure.IntegrationTests.csproj
 │
 └── Api.IntegrationTests
       ├── Controllers
       │     ├── ProjectsControllerTests.cs
       │     ├── TasksControllerTests.cs
       │     ├── TeamsControllerTests.cs
       │     └── AuthControllerTests.cs
       ├── Endpoints
       │     ├── ProjectEndpointsTests.cs
       │     ├── TaskEndpointsTests.cs
       │     └── TeamEndpointsTests.cs
       ├── Scenarios
       │     ├── ProjectManagementScenarioTests.cs
       │     ├── TaskWorkflowScenarioTests.cs
       │     └── TeamCollaborationScenarioTests.cs
       ├── Middleware
       │     ├── ExceptionHandlingMiddlewareTests.cs
       │     └── AuthenticationTests.cs
       ├── Common
       │     ├── CustomWebApplicationFactory.cs
       │     ├── ApiTestBase.cs
       │     ├── AuthHelper.cs
       │     └── HttpClientExtensions.cs
       └── Api.IntegrationTests.csproj

========================================
PACKAGE REFERENCES
========================================

**Domain.UnitTests.csproj:**

Required NuGet packages:
- xunit (2.9.x or later)
- xunit.runner.visualstudio (2.8.x or later)
- Microsoft.NET.Test.Sdk (17.x or later)
- FluentAssertions (6.x or later)
- Moq (4.x or later)
- AutoFixture (4.x or later)
- AutoFixture.Xunit2 (4.x or later)
- Bogus (35.x or later)
- coverlet.collector (6.x or later)

Project References:
- Domain project

**Application.UnitTests.csproj:**

Same packages as Domain.UnitTests plus:
- AutoMapper (13.x or later)
- FluentValidation (11.x or later)
- MediatR (13.x or later)

Project References:
- Application project
- Domain project

**Infrastructure.IntegrationTests.csproj:**

Required NuGet packages:
- xunit (2.9.x or later)
- xunit.runner.visualstudio (2.8.x or later)
- Microsoft.NET.Test.Sdk (17.x or later)
- FluentAssertions (6.x or later)
- Testcontainers (3.x or later)
- Testcontainers.PostgreSql (3.x or later)
- Respawn (6.x or later)
- Npgsql.EntityFrameworkCore.PostgreSQL (10.x or later)
- Bogus (35.x or later)
- coverlet.collector (6.x or later)

Project References:
- Infrastructure project
- Application project
- Domain project

**Api.IntegrationTests.csproj:**

Same packages as Infrastructure.IntegrationTests plus:
- Microsoft.AspNetCore.Mvc.Testing (10.x or later)
- Microsoft.AspNetCore.TestHost (10.x or later)
- System.IdentityModel.Tokens.Jwt (for token generation)

Project References:
- WebApi project
- Infrastructure project
- Application project
- Domain project

========================================
UNIT TEST BASE CLASSES
========================================

**CommandHandlerTestBase:**
```csharp
public abstract class CommandHandlerTestBase : IDisposable
{
    protected readonly IFixture Fixture;
    protected readonly Mock<IUnitOfWork> MockUnitOfWork;

    protected CommandHandlerTestBase()
    {
        Fixture = new Fixture();
        Fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        
        MockUnitOfWork = new Mock<IUnitOfWork>();
        MockUnitOfWork
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);
    }

    public void Dispose()
    {
        // Cleanup if needed
        GC.SuppressFinalize(this);
    }
}
```

**QueryHandlerTestBase:**
```csharp
public abstract class QueryHandlerTestBase : IDisposable
{
    protected readonly IFixture Fixture;
    protected readonly IMapper Mapper;

    protected QueryHandlerTestBase()
    {
        Fixture = new Fixture();
        Fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        
        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MappingProfile>();
        });
        
        Mapper = configuration.CreateMapper();
    }

    public void Dispose()
    {
        // Cleanup if needed
        GC.SuppressFinalize(this);
    }
}
```

========================================
BUILDER PATTERN FOR TEST DATA
========================================

**ProjectBuilder:**
```csharp
public class ProjectBuilder
{
    private string _name = "Test Project";
    private string? _description = "Test Description";
    private int _ownerId = 1;
    private ProjectStatus _status = ProjectStatus.Active;
    private readonly List<TaskItem> _tasks = new();

    public ProjectBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    public ProjectBuilder WithDescription(string description)
    {
        _description = description;
        return this;
    }

    public ProjectBuilder WithOwnerId(int ownerId)
    {
        _ownerId = ownerId;
        return this;
    }

    public ProjectBuilder WithStatus(ProjectStatus status)
    {
        _status = status;
        return this;
    }

    public ProjectBuilder Archived()
    {
        _status = ProjectStatus.Archived;
        return this;
    }

    public ProjectBuilder WithTask(TaskItem task)
    {
        _tasks.Add(task);
        return this;
    }

    public ProjectBuilder WithTasks(int count)
    {
        for (int i = 0; i < count; i++)
        {
            var task = new TaskItemBuilder()
                .WithTitle($"Task {i + 1}")
                .Build();
            _tasks.Add(task);
        }
        return this;
    }

    public Project Build()
    {
        var project = Project.Create(_name, _description, _ownerId);
        
        if (_status == ProjectStatus.Archived)
        {
            project.Archive();
        }

        foreach (var task in _tasks)
        {
            project.AddTask(task);
        }

        return project;
    }
}
```

Follow same pattern for TaskItemBuilder, TeamBuilder, TeamMemberBuilder.

========================================
DOMAIN UNIT TEST EXAMPLES
========================================

**ProjectTests:**
```csharp
public class ProjectTests
{
    private readonly Faker _faker;

    public ProjectTests()
    {
        _faker = new Faker();
    }

    public class Constructor : ProjectTests
    {
        [Fact]
        public void Create_WithValidData_ShouldCreateProject()
        {
            // Arrange
            var name = _faker.Company.CompanyName();
            var description = _faker.Lorem.Sentence();
            var ownerId = _faker.Random.Int(1, 1000);

            // Act
            var project = Project.Create(name, description, ownerId);

            // Assert
            project.Should().NotBeNull();
            project.Name.Should().Be(name);
            project.Description.Should().Be(description);
            project.OwnerId.Should().Be(ownerId);
            project.Status.Should().Be(ProjectStatus.Active);
            project.IsDeleted.Should().BeFalse();
            project.CreatedAt.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromSeconds(1));
        }

        [Fact]
        public void Create_WithEmptyName_ShouldThrowDomainException()
        {
            // Arrange
            var name = string.Empty;
            var description = _faker.Lorem.Sentence();
            var ownerId = _faker.Random.Int(1, 1000);

            // Act
            Action act = () => Project.Create(name, description, ownerId);

            // Assert
            act.Should().Throw<DomainException>()
                .WithMessage("*name*");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Create_WithInvalidName_ShouldThrowDomainException(string invalidName)
        {
            // Arrange
            var description = _faker.Lorem.Sentence();
            var ownerId = _faker.Random.Int(1, 1000);

            // Act
            Action act = () => Project.Create(invalidName, description, ownerId);

            // Assert
            act.Should().Throw<DomainException>();
        }

        [Fact]
        public void Create_ShouldRaiseDomainEvent()
        {
            // Arrange
            var name = _faker.Company.CompanyName();
            var description = _faker.Lorem.Sentence();
            var ownerId = _faker.Random.Int(1, 1000);

            // Act
            var project = Project.Create(name, description, ownerId);

            // Assert
            project.DomainEvents.Should().ContainSingle();
            project.DomainEvents.First().Should().BeOfType<ProjectCreatedEvent>();
        }
    }

    public class ArchiveMethod : ProjectTests
    {
        [Fact]
        public void Archive_WithNoActiveTasks_ShouldArchiveProject()
        {
            // Arrange
            var project = new ProjectBuilder()
                .WithName("Test Project")
                .Build();

            // Act
            project.Archive();

            // Assert
            project.Status.Should().Be(ProjectStatus.Archived);
            project.UpdatedAt.Should().NotBeNull();
            project.DomainEvents.Should().Contain(e => e is ProjectArchivedEvent);
        }

        [Fact]
        public void Archive_WithActiveTasks_ShouldThrowDomainException()
        {
            // Arrange
            var project = new ProjectBuilder()
                .WithTasks(5)
                .Build();

            // Act
            Action act = () => project.Archive();

            // Assert
            act.Should().Throw<DomainException>()
                .WithMessage("*active tasks*");
        }
    }
}
```

**MoneyTests (Value Object):**
```csharp
public class MoneyTests
{
    public class Constructor
    {
        [Fact]
        public void Create_WithValidData_ShouldCreateMoney()
        {
            // Arrange
            var amount = 100.50m;
            var currency = Currency.USD;

            // Act
            var money = new Money(amount, currency);

            // Assert
            money.Amount.Should().Be(amount);
            money.Currency.Should().Be(currency);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(-100.50)]
        public void Create_WithNegativeAmount_ShouldThrowDomainException(decimal invalidAmount)
        {
            // Arrange
            var currency = Currency.USD;

            // Act
            Action act = () => new Money(invalidAmount, currency);

            // Assert
            act.Should().Throw<DomainException>()
                .WithMessage("*negative*");
        }
    }

    public class EqualityTests
    {
        [Fact]
        public void Equals_WithSameValues_ShouldBeEqual()
        {
            // Arrange
            var money1 = new Money(100m, Currency.USD);
            var money2 = new Money(100m, Currency.USD);

            // Act & Assert
            money1.Should().Be(money2);
            (money1 == money2).Should().BeTrue();
        }

        [Fact]
        public void Equals_WithDifferentAmounts_ShouldNotBeEqual()
        {
            // Arrange
            var money1 = new Money(100m, Currency.USD);
            var money2 = new Money(200m, Currency.USD);

            // Act & Assert
            money1.Should().NotBe(money2);
            (money1 != money2).Should().BeTrue();
        }

        [Fact]
        public void GetHashCode_WithSameValues_ShouldBeSame()
        {
            // Arrange
            var money1 = new Money(100m, Currency.USD);
            var money2 = new Money(100m, Currency.USD);

            // Act & Assert
            money1.GetHashCode().Should().Be(money2.GetHashCode());
        }
    }
}
```

========================================
APPLICATION UNIT TEST EXAMPLES
========================================

**CreateProjectCommandHandlerTests:**
```csharp
public class CreateProjectCommandHandlerTests : CommandHandlerTestBase
{
    private readonly Mock<IProjectRepository> _mockProjectRepository;
    private readonly CreateProjectCommandHandler _handler;

    public CreateProjectCommandHandlerTests()
    {
        _mockProjectRepository = new Mock<IProjectRepository>();
        _handler = new CreateProjectCommandHandler(
            _mockProjectRepository.Object,
            MockUnitOfWork.Object);
    }

    [Fact]
    public async Task Handle_WithValidCommand_ShouldCreateProject()
    {
        // Arrange
        var command = new CreateProjectCommand(
            "Test Project",
            "Test Description",
            1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeGreaterThan(0);

        _mockProjectRepository.Verify(
            x => x.AddAsync(It.IsAny<Project>(), It.IsAny<CancellationToken>()),
            Times.Once);

        MockUnitOfWork.Verify(
            x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldCreateProjectWithCorrectProperties()
    {
        // Arrange
        var command = new CreateProjectCommand(
            "Test Project",
            "Test Description",
            1);

        Project? capturedProject = null;
        _mockProjectRepository
            .Setup(x => x.AddAsync(It.IsAny<Project>(), It.IsAny<CancellationToken>()))
            .Callback<Project, CancellationToken>((project, _) => capturedProject = project);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        capturedProject.Should().NotBeNull();
        capturedProject!.Name.Should().Be(command.Name);
        capturedProject.Description.Should().Be(command.Description);
        capturedProject.OwnerId.Should().Be(command.OwnerId);
    }
}
```

**CreateProjectCommandValidatorTests:**
```csharp
public class CreateProjectCommandValidatorTests
{
    private readonly CreateProjectCommandValidator _validator;

    public CreateProjectCommandValidatorTests()
    {
        _validator = new CreateProjectCommandValidator();
    }

    [Fact]
    public void Validate_WithValidCommand_ShouldNotHaveErrors()
    {
        // Arrange
        var command = new CreateProjectCommand(
            "Test Project",
            "Test Description",
            1);

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Validate_WithInvalidName_ShouldHaveValidationError(string invalidName)
    {
        // Arrange
        var command = new CreateProjectCommand(invalidName, "Description", 1);

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(CreateProjectCommand.Name));
    }

    [Fact]
    public void Validate_WithNameTooLong_ShouldHaveValidationError()
    {
        // Arrange
        var longName = new string('a', 201);
        var command = new CreateProjectCommand(longName, "Description", 1);

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => 
            e.PropertyName == nameof(CreateProjectCommand.Name) &&
            e.ErrorMessage.Contains("200"));
    }
}
```

**ValidationBehaviorTests:**
```csharp
public class ValidationBehaviorTests
{
    private readonly Mock<IValidator<TestCommand>> _mockValidator;
    private readonly ValidationBehavior<TestCommand, Result<int>> _behavior;

    public ValidationBehaviorTests()
    {
        _mockValidator = new Mock<IValidator<TestCommand>>();
        _behavior = new ValidationBehavior<TestCommand, Result<int>>(
            new[] { _mockValidator.Object });
    }

    [Fact]
    public async Task Handle_WithNoValidators_ShouldContinuePipeline()
    {
        // Arrange
        var behavior = new ValidationBehavior<TestCommand, Result<int>>(
            Enumerable.Empty<IValidator<TestCommand>>());
        
        var command = new TestCommand();
        var expectedResult = Result<int>.Success(42);
        
        RequestHandlerDelegate<Result<int>> next = () => Task.FromResult(expectedResult);

        // Act
        var result = await behavior.Handle(command, next, CancellationToken.None);

        // Assert
        result.Should().Be(expectedResult);
    }

    [Fact]
    public async Task Handle_WithValidCommand_ShouldContinuePipeline()
    {
        // Arrange
        var command = new TestCommand();
        var validationResult = new ValidationResult();
        
        _mockValidator
            .Setup(x => x.ValidateAsync(
                It.IsAny<ValidationContext<TestCommand>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        var expectedResult = Result<int>.Success(42);
        RequestHandlerDelegate<Result<int>> next = () => Task.FromResult(expectedResult);

        // Act
        var result = await _behavior.Handle(command, next, CancellationToken.None);

        // Assert
        result.Should().Be(expectedResult);
    }

    [Fact]
    public async Task Handle_WithInvalidCommand_ShouldThrowValidationException()
    {
        // Arrange
        var command = new TestCommand();
        var validationFailures = new List<ValidationFailure>
        {
            new ValidationFailure("Property", "Error message")
        };
        var validationResult = new ValidationResult(validationFailures);

        _mockValidator
            .Setup(x => x.ValidateAsync(
                It.IsAny<ValidationContext<TestCommand>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        RequestHandlerDelegate<Result<int>> next = () => 
            Task.FromResult(Result<int>.Success(42));

        // Act
        Func<Task> act = async () => 
            await _behavior.Handle(command, next, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<Application.Common.Exceptions.ValidationException>()
            .Where(ex => ex.Errors.ContainsKey("Property"));
    }

    private record TestCommand : IRequest<Result<int>>;
}
```

**MappingProfileTests:**
```csharp
public class MappingProfileTests
{
    private readonly IConfigurationProvider _configuration;
    private readonly IMapper _mapper;

    public MappingProfileTests()
    {
        _configuration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MappingProfile>();
        });

        _mapper = _configuration.CreateMapper();
    }

    [Fact]
    public void MappingProfile_ShouldBeValid()
    {
        // Assert
        _configuration.AssertConfigurationIsValid();
    }

    [Fact]
    public void Map_ProjectToProjectDto_ShouldMapCorrectly()
    {
        // Arrange
        var project = new ProjectBuilder()
            .WithName("Test Project")
            .WithDescription("Test Description")
            .WithOwnerId(1)
            .Build();

        // Act
        var dto = _mapper.Map<ProjectDto>(project);

        // Assert
        dto.Should().NotBeNull();
        dto.Id.Should().Be(project.Id);
        dto.Name.Should().Be(project.Name);
        dto.Status.Should().Be(project.Status);
        dto.OwnerId.Should().Be(project.OwnerId);
        dto.CreatedAt.Should().Be(project.CreatedAt);
    }

    [Fact]
    public void Map_ProjectToProjectDetailDto_ShouldMapWithTasks()
    {
        // Arrange
        var project = new ProjectBuilder()
            .WithName("Test Project")
            .WithTasks(3)
            .Build();

        // Act
        var dto = _mapper.Map<ProjectDetailDto>(project);

        // Assert
        dto.Should().NotBeNull();
        dto.TaskCount.Should().Be(3);
        dto.Tasks.Should().HaveCount(3);
    }
}
```

========================================
INTEGRATION TEST SETUP
========================================

**DatabaseFixture (Testcontainers):**
```csharp
public class DatabaseFixture : IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgresContainer;
    private Respawner _respawner = null!;
    private DbConnection _dbConnection = null!;

    public string ConnectionString { get; private set; } = string.Empty;

    public DatabaseFixture()
    {
        _postgresContainer = new PostgreSqlBuilder()
            .WithImage("postgres:16-alpine")
            .WithDatabase("taskmanagement_test")
            .WithUsername("postgres")
            .WithPassword("postgres")
            .WithCleanUp(true)
            .Build();
    }

    public async Task InitializeAsync()
    {
        await _postgresContainer.StartAsync();
        
        ConnectionString = _postgresContainer.GetConnectionString();

        // Run migrations
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseNpgsql(ConnectionString)
            .Options;

        await using var context = new ApplicationDbContext(options, null!);
        await context.Database.MigrateAsync();

        // Initialize Respawner
        _dbConnection = new NpgsqlConnection(ConnectionString);
        await _dbConnection.OpenAsync();

        _respawner = await Respawner.CreateAsync(_dbConnection, new RespawnerOptions
        {
            DbAdapter = DbAdapter.Postgres,
            SchemasToInclude = new[] { "public" },
            TablesToIgnore = new[] { new Table("__EFMigrationsHistory") }
        });
    }

    public async Task ResetDatabaseAsync()
    {
        await _respawner.ResetAsync(_dbConnection);
    }

    public async Task DisposeAsync()
    {
        await _dbConnection.DisposeAsync();
        await _postgresContainer.DisposeAsync();
    }

    public ApplicationDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseNpgsql(ConnectionString)
            .Options;

        return new ApplicationDbContext(options, null!);
    }
}

[CollectionDefinition(nameof(DatabaseCollection))]
public class DatabaseCollection : ICollectionFixture<DatabaseFixture>
{
}
```

**IntegrationTestBase:**
```csharp
[Collection(nameof(DatabaseCollection))]
public abstract class IntegrationTestBase : IAsyncLifetime
{
    protected readonly DatabaseFixture DatabaseFixture;
    protected readonly ApplicationDbContext DbContext;
    protected readonly Faker Faker;

    protected IntegrationTestBase(DatabaseFixture databaseFixture)
    {
        DatabaseFixture = databaseFixture;
        DbContext = databaseFixture.CreateDbContext();
        Faker = new Faker();
    }

    public virtual Task InitializeAsync() => Task.CompletedTask;

    public virtual async Task DisposeAsync()
    {
        await DbContext.DisposeAsync();
        await DatabaseFixture.ResetDatabaseAsync();
    }

    protected async Task<T> AddAsync<T>(T entity) where T : class
    {
        await DbContext.Set<T>().AddAsync(entity);
        await DbContext.SaveChangesAsync();
        DbContext.Entry(entity).State = EntityState.Detached;
        return entity;
    }

    protected async Task<T?> FindAsync<T>(params object[] keyValues) where T : class
    {
        return await DbContext.Set<T>().FindAsync(keyValues);
    }
}
```

========================================
INFRASTRUCTURE INTEGRATION TESTS
========================================

**ApplicationDbContextTests:**
```csharp
public class ApplicationDbContextTests : IntegrationTestBase
{
    public ApplicationDbContextTests(DatabaseFixture databaseFixture) 
        : base(databaseFixture)
    {
    }

    [Fact]
    public async Task SaveChangesAsync_ShouldPersistEntity()
    {
        // Arrange
        var project = Project.Create("Test Project", "Description", 1);

        // Act
        await DbContext.Projects.AddAsync(project);
        var result = await DbContext.SaveChangesAsync();

        // Assert
        result.Should().BeGreaterThan(0);
        
        var savedProject = await DbContext.Projects.FindAsync(project.Id);
        savedProject.Should().NotBeNull();
        savedProject!.Name.Should().Be("Test Project");
    }

    [Fact]
    public async Task SoftDeletedEntities_ShouldBeFilteredByDefault()
    {
        // Arrange
        var project = Project.Create("Test Project", null, 1);
        await DbContext.Projects.AddAsync(project);
        await DbContext.SaveChangesAsync();

        project.SoftDelete();
        await DbContext.SaveChangesAsync();
        DbContext.ChangeTracker.Clear();

        // Act
        var projects = await DbContext.Projects.ToListAsync();

        // Assert
        projects.Should().BeEmpty();
    }

    [Fact]
    public async Task CanQuerySoftDeletedEntitiesWithIgnoreQueryFilters()
    {
        // Arrange
        var project = Project.Create("Test Project", null, 1);
        await DbContext.Projects.AddAsync(project);
        await DbContext.SaveChangesAsync();

        project.SoftDelete();
        await DbContext.SaveChangesAsync();
        DbContext.ChangeTracker.Clear();

        // Act
        var projects = await DbContext.Projects
            .IgnoreQueryFilters()
            .ToListAsync();

        // Assert
        projects.Should().HaveCount(1);
        projects.First().IsDeleted.Should().BeTrue();
    }
}
```

**ProjectRepositoryTests:**
```csharp
public class ProjectRepositoryTests : IntegrationTestBase
{
    private readonly IProjectRepository _repository;

    public ProjectRepositoryTests(DatabaseFixture databaseFixture) 
        : base(databaseFixture)
    {
        _repository = new ProjectRepository(DbContext);
    }

    [Fact]
    public async Task AddAsync_ShouldAddProject()
    {
        // Arrange
        var project = Project.Create("Test Project", "Description", 1);

        // Act
        await _repository.AddAsync(project, CancellationToken.None);
        await DbContext.SaveChangesAsync();

        // Assert
        var savedProject = await DbContext.Projects.FindAsync(project.Id);
        savedProject.Should().NotBeNull();
        savedProject!.Name.Should().Be("Test Project");
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnProjectWithTasks()
    {
        // Arrange
        var project = Project.Create("Test Project", null, 1);
        var task1 = TaskItem.Create("Task 1", null, project.Id, Priority.High);
        var task2 = TaskItem.Create("Task 2", null, project.Id, Priority.Low);
        
        project.AddTask(task1);
        project.AddTask(task2);

        await DbContext.Projects.AddAsync(project);
        await DbContext.SaveChangesAsync();
        DbContext.ChangeTracker.Clear();

        // Act
        var result = await _repository.GetByIdAsync(project.Id, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Tasks.Should().HaveCount(2);
    }

    [Fact]
    public async Task WhereAsync_ShouldFilterProjects()
    {
        // Arrange
        var project1 = Project.Create("Project 1", null, 1);
        var project2 = Project.Create("Project 2", null, 1);
        var project3 = Project.Create("Project 3", null, 2);

        await DbContext.Projects.AddRangeAsync(project1, project2, project3);
        await DbContext.SaveChangesAsync();
        DbContext.ChangeTracker.Clear();

        // Act
        var results = await _repository.WhereAsync(
            p => p.OwnerId == 1, 
            CancellationToken.None);

        // Assert
        results.Should().HaveCount(2);
        results.Should().AllSatisfy(p => p.OwnerId.Should().Be(1));
    }
}
```

**AuditableEntityInterceptorTests:**
```csharp
public class AuditableEntityInterceptorTests : IntegrationTestBase
{
    public AuditableEntityInterceptorTests(DatabaseFixture databaseFixture) 
        : base(databaseFixture)
    {
    }

    [Fact]
    public async Task AddEntity_ShouldSetCreatedAt()
    {
        // Arrange
        var beforeCreate = DateTimeOffset.UtcNow;
        var project = Project.Create("Test Project", null, 1);

        // Act
        await DbContext.Projects.AddAsync(project);
        await DbContext.SaveChangesAsync();
        var afterCreate = DateTimeOffset.UtcNow;

        // Assert
        project.CreatedAt.Should().BeOnOrAfter(beforeCreate);
        project.CreatedAt.Should().BeOnOrBefore(afterCreate);
        project.UpdatedAt.Should().BeNull();
    }

    [Fact]
    public async Task UpdateEntity_ShouldSetUpdatedAt()
    {
        // Arrange
        var project = Project.Create("Test Project", null, 1);
        await DbContext.Projects.AddAsync(project);
        await DbContext.SaveChangesAsync();

        var createdAt = project.CreatedAt;
        await Task.Delay(10);

        // Act
        var beforeUpdate = DateTimeOffset.UtcNow;
        project.UpdateDetails("Updated Name", null);
        await DbContext.SaveChangesAsync();
        var afterUpdate = DateTimeOffset.UtcNow;

        // Assert
        project.CreatedAt.Should().Be(createdAt);
        project.UpdatedAt.Should().NotBeNull();
        project.UpdatedAt.Should().BeOnOrAfter(beforeUpdate);
        project.UpdatedAt.Should().BeOnOrBefore(afterUpdate);
    }
}
```

========================================
API INTEGRATION TESTS SETUP
========================================

**CustomWebApplicationFactory:**
```csharp
public class CustomWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgresContainer;
    private Respawner _respawner = null!;
    private DbConnection _dbConnection = null!;

    public string ConnectionString { get; private set; } = string.Empty;

    public CustomWebApplicationFactory()
    {
        _postgresContainer = new PostgreSqlBuilder()
            .WithImage("postgres:16-alpine")
            .WithDatabase("taskmanagement_api_test")
            .WithUsername("postgres")
            .WithPassword("postgres")
            .WithCleanUp(true)
            .Build();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));
            
            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseNpgsql(ConnectionString);
            });

            services.Configure<JwtSettings>(options =>
            {
                options.Key = "TestKeyThatIsAtLeast32CharactersLong!";
                options.Issuer = "TestIssuer";
                options.Audience = "TestAudience";
                options.ExpiryMinutes = 60;
            });
        });

        builder.UseEnvironment("Testing");
    }

    public async Task InitializeAsync()
    {
        await _postgresContainer.StartAsync();
        
        ConnectionString = _postgresContainer.GetConnectionString();

        using var scope = Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        await dbContext.Database.MigrateAsync();

        _dbConnection = new NpgsqlConnection(ConnectionString);
        await _dbConnection.OpenAsync();

        _respawner = await Respawner.CreateAsync(_dbConnection, new RespawnerOptions
        {
            DbAdapter = DbAdapter.Postgres,
            SchemasToInclude = new[] { "public" },
            TablesToIgnore = new[] { new Table("__EFMigrationsHistory") }
        });
    }

    public async Task ResetDatabaseAsync()
    {
        await _respawner.ResetAsync(_dbConnection);
    }

    async Task IAsyncLifetime.DisposeAsync()
    {
        await _dbConnection.DisposeAsync();
        await _postgresContainer.DisposeAsync();
    }
}

[CollectionDefinition(nameof(ApiCollection))]
public class ApiCollection : ICollectionFixture<CustomWebApplicationFactory>
{
}
```

**ApiTestBase:**
```csharp
[Collection(nameof(ApiCollection))]
public abstract class ApiTestBase : IAsyncLifetime
{
    protected readonly CustomWebApplicationFactory Factory;
    protected readonly HttpClient Client;
    protected readonly Faker Faker;

    protected ApiTestBase(CustomWebApplicationFactory factory)
    {
        Factory = factory;
        Client = factory.CreateClient();
        Faker = new Faker();
    }

    public virtual Task InitializeAsync() => Task.CompletedTask;

    public virtual async Task DisposeAsync()
    {
        await Factory.ResetDatabaseAsync();
    }

    protected async Task<string> GetAuthTokenAsync(string email = "test@test.com", string password = "Test123!")
    {
        var registerRequest = new
        {
            Email = email,
            Password = password,
            FirstName = "Test",
            LastName = "User"
        };

        await Client.PostAsJsonAsync("/api/v1/auth/register", registerRequest);

        var loginRequest = new
        {
            Email = email,
            Password = password
        };

        var response = await Client.PostAsJsonAsync("/api/v1/auth/login", loginRequest);
        var result = await response.Content.ReadFromJsonAsync<ApiResponse<AuthResponse>>();

        return result!.Data!.AccessToken;
    }

    protected void SetAuthToken(string token)
    {
        Client.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", token);
    }
}
```

========================================
API INTEGRATION TESTS
========================================

**ProjectsControllerTests:**
```csharp
public class ProjectsControllerTests : ApiTestBase
{
    public ProjectsControllerTests(CustomWebApplicationFactory factory) 
        : base(factory)
    {
    }

    [Fact]
    public async Task CreateProject_WithValidData_ShouldReturnSuccess()
    {
        // Arrange
        var token = await GetAuthTokenAsync();
        SetAuthToken(token);

        var request = new
        {
            Name = "Test Project",
            Description = "Test Description",
            OwnerId = 1
        };

        // Act
        var response = await Client.PostAsJsonAsync("/api/v1/projects", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var result = await response.Content.ReadFromJsonAsync<ApiResponse<int>>();
        result.Should().NotBeNull();
        result!.Success.Should().BeTrue();
        result.Data.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task CreateProject_WithoutAuthentication_ShouldReturnUnauthorized()
    {
        // Arrange
        var request = new
        {
            Name = "Test Project",
            Description = "Test Description",
            OwnerId = 1
        };

        // Act
        var response = await Client.PostAsJsonAsync("/api/v1/projects", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task CreateProject_WithInvalidData_ShouldReturnBadRequest()
    {
        // Arrange
        var token = await GetAuthTokenAsync();
        SetAuthToken(token);

        var request = new
        {
            Name = "",
            Description = "Test Description",
            OwnerId = 1
        };

        // Act
        var response = await Client.PostAsJsonAsync("/api/v1/projects", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        
        var result = await response.Content.ReadFromJsonAsync<ValidationErrorResponse>();
        result.Should().NotBeNull();
        result!.Errors.Should().ContainKey("Name");
    }

    [Fact]
    public async Task GetProjectById_WithExistingId_ShouldReturnProject()
    {
        // Arrange
        var token = await GetAuthTokenAsync();
        SetAuthToken(token);

        var createRequest = new
        {
            Name = "Test Project",
            Description = "Test Description",
            OwnerId = 1
        };

        var createResponse = await Client.PostAsJsonAsync("/api/v1/projects", createRequest);
        var createResult = await createResponse.Content.ReadFromJsonAsync<ApiResponse<int>>();
        var projectId = createResult!.Data;

        // Act
        var response = await Client.GetAsync($"/api/v1/projects/{projectId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var result = await response.Content.ReadFromJsonAsync<ApiResponse<ProjectDetailDto>>();
        result.Should().NotBeNull();
        result!.Data.Should().NotBeNull();
        result.Data!.Name.Should().Be("Test Project");
    }
}
```

**ProjectManagementScenarioTests:**
```csharp
public class ProjectManagementScenarioTests : ApiTestBase
{
    public ProjectManagementScenarioTests(CustomWebApplicationFactory factory) 
        : base(factory)
    {
    }

    [Fact]
    public async Task CompleteProjectWorkflow_ShouldSucceed()
    {
        // Arrange
        var token = await GetAuthTokenAsync();
        SetAuthToken(token);

        // Act & Assert

        // 1. Create a project
        var createProjectRequest = new
        {
            Name = "New Project",
            Description = "Project Description",
            OwnerId = 1
        };

        var createProjectResponse = await Client.PostAsJsonAsync(
            "/api/v1/projects", 
            createProjectRequest);
        createProjectResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var projectResult = await createProjectResponse.Content
            .ReadFromJsonAsync<ApiResponse<int>>();
        var projectId = projectResult!.Data;

        // 2. Add tasks to project
        var createTaskRequest1 = new
        {
            Title = "Task 1",
            Description = "Task 1 Description",
            ProjectId = projectId,
            Priority = "High",
            DueDate = DateTimeOffset.UtcNow.AddDays(7)
        };

        var createTaskResponse1 = await Client.PostAsJsonAsync(
            "/api/v1/tasks", 
            createTaskRequest1);
        createTaskResponse1.StatusCode.Should().Be(HttpStatusCode.OK);

        var taskResult1 = await createTaskResponse1.Content
            .ReadFromJsonAsync<ApiResponse<int>>();
        var task1Id = taskResult1!.Data;

        // 3. Assign task
        var assignTaskRequest = new
        {
            AssigneeId = 1
        };

        var assignResponse = await Client.PutAsJsonAsync(
            $"/api/v1/tasks/{task1Id}/assign", 
            assignTaskRequest);
        assignResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        // 4. Complete task
        var completeResponse = await Client.PutAsync(
            $"/api/v1/tasks/{task1Id}/complete", 
            null);
        completeResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        // 5. Verify task is completed
        var getTaskResponse = await Client.GetAsync($"/api/v1/tasks/{task1Id}");
        var taskDto = await getTaskResponse.Content
            .ReadFromJsonAsync<ApiResponse<TaskDetailDto>>();
        taskDto!.Data!.Status.Should().Be(TaskStatus.Done);

        // 6. Get project with completed task
        var getProjectResponse = await Client.GetAsync($"/api/v1/projects/{projectId}");
        var projectDto = await getProjectResponse.Content
            .ReadFromJsonAsync<ApiResponse<ProjectDetailDto>>();
        projectDto!.Data!.Tasks.Should().HaveCount(1);
        projectDto.Data.Tasks.First().Status.Should().Be(TaskStatus.Done);
    }
}
```

========================================
IMPLEMENTATION CONSTRAINTS
========================================

**Unit Tests:**
- One assertion concept per test
- Arrange-Act-Assert pattern strictly
- Mock only direct dependencies
- Use FluentAssertions for all assertions
- Use Bogus for realistic test data
- Use AutoFixture for complex object graphs
- Use builders for domain entities
- Test both happy path and edge cases
- Test all validation rules
- Test all business rules
- Test all domain events
- Test exception scenarios
- No test interdependencies
- No shared mutable state
- Fast execution
- Deterministic tests

**Integration Tests:**
- Use real database (Testcontainers PostgreSQL)
- Clean database between tests (Respawn)
- Test against actual API (WebApplicationFactory)
- No mocking of infrastructure
- Test complete request/response cycle
- Verify database state after operations
- Test authentication and authorization
- Test middleware behavior
- Test validation at API level
- Test error handling and responses
- Test pagination and filtering
- Test concurrent requests where relevant
- Test transaction behavior
- Test domain event publishing
- Use realistic test data
- Dispose resources properly
- Use IAsyncLifetime for setup/cleanup

========================================
EXPECTED OUTPUT FILES
========================================

**Domain.UnitTests (~30 files)**
**Application.UnitTests (~90 files)**
**Infrastructure.IntegrationTests (~25 files)**
**Api.IntegrationTests (~30 files)**

Total: ~175 test files

========================================
OUTPUT FORMAT
========================================

Return full C# code for all test files.

Each file must be separated with:

// File: tests/{ProjectName}.UnitTests/...
// File: tests/{ProjectName}.IntegrationTests/...

Use proper namespaces following the folder structure pattern:
- TaskManagement.Domain.UnitTests.*
- TaskManagement.Application.UnitTests.*
- TaskManagement.Infrastructure.IntegrationTests.*
- TaskManagement.Api.IntegrationTests.*

Include all necessary using statements.
Reference production code namespaces where needed.

Only return code. No explanations. No markdown. No comments outside code.

Begin output now.