# Web API Layer - Clean Architecture Prompt

You are a senior .NET software architect.

Generate a complete Web API/Presentation Layer using:

- Clean Architecture
- ASP.NET Core Web API
- .NET 10
- C#
- Controller-based API (enterprise standard)
- JWT Authentication
- Role-based Authorization
- API Versioning
- Swagger/OpenAPI
- Global Exception Handling
- CORS
- Rate Limiting
- Health Checks
- Request/Response Logging
- Validation
- Problem Details (RFC 7807)

The output must ONLY contain the Web API Layer code.
No explanations. No comments outside code.
Follow the folder structure and rules exactly.

========================================
BUSINESS DOMAIN CONTEXT
========================================

This is a Task Management System Web API that exposes:

**Project Endpoints:**
- POST /api/v1/projects - Create project
- PUT /api/v1/projects/{id}/archive - Archive project
- PUT /api/v1/projects/{id} - Update project
- GET /api/v1/projects/{id} - Get project by id
- GET /api/v1/projects - Get all projects (paginated)
- GET /api/v1/projects/owner/{ownerId} - Get projects by owner

**Task Endpoints:**
- POST /api/v1/tasks - Create task
- PUT /api/v1/tasks/{id}/assign - Assign task
- PUT /api/v1/tasks/{id}/status - Update task status
- PUT /api/v1/tasks/{id}/complete - Complete task
- PUT /api/v1/tasks/{id} - Update task
- GET /api/v1/tasks/{id} - Get task by id
- GET /api/v1/tasks/project/{projectId} - Get tasks by project
- GET /api/v1/tasks/assignee/{assigneeId} - Get tasks by assignee
- GET /api/v1/tasks/overdue - Get overdue tasks

**Team Endpoints:**
- POST /api/v1/teams - Create team
- POST /api/v1/teams/{id}/members - Add team member
- DELETE /api/v1/teams/{id}/members/{userId} - Remove team member
- PUT /api/v1/teams/{id}/leader - Change team leader
- GET /api/v1/teams/{id} - Get team by id
- GET /api/v1/teams - Get all teams (paginated)
- GET /api/v1/teams/leader/{leaderId} - Get teams by leader

**Authentication Endpoints:**
- POST /api/v1/auth/register - Register user
- POST /api/v1/auth/login - Login user
- POST /api/v1/auth/refresh - Refresh token
- POST /api/v1/auth/revoke - Revoke token

**Health Check Endpoints:**
- GET /health - Overall health
- GET /health/ready - Readiness probe
- GET /health/live - Liveness probe

========================================
ARCHITECTURE RULES
========================================

- Web API layer must be inside: src/WebApi or src/Api
- Depends on Application and Infrastructure layers
- No business logic in controllers (thin controllers)
- Controllers only orchestrate (call MediatR)
- Use MediatR for all command/query execution
- API responses follow consistent format
- All endpoints return proper HTTP status codes
- Use ActionResult<T> for typed responses
- Async/await for all endpoints
- CancellationToken support
- Versioning via URL (v1, v2)
- JWT Bearer authentication
- Role-based authorization attributes
- Global exception handler middleware
- Model validation via FluentValidation (already in Application)
- API validation via Data Annotations (additional layer)
- Swagger documentation for all endpoints
- XML comments for Swagger
- Health checks for dependencies (database, external services)
- CORS configuration
- Rate limiting per endpoint
- Request/response logging middleware
- Correlation ID tracking
- Problem Details (RFC 7807) for errors
- OpenAPI specification generation
- Environment-based configuration
- Secrets management (AWS Secrets Manager, User Secrets)

========================================
FOLDER STRUCTURE
========================================

src
 └── WebApi (or Api)
       ├── Controllers
       │     ├── V1
       │     │     ├── ProjectsController.cs
       │     │     ├── TasksController.cs
       │     │     ├── TeamsController.cs
       │     │     └── AuthController.cs
       │     └── BaseApiController.cs
       │
       ├── Middleware
       │     ├── ExceptionHandlingMiddleware.cs
       │     ├── RequestLoggingMiddleware.cs
       │     └── CorrelationIdMiddleware.cs
       │
       ├── Filters
       │     ├── ApiExceptionFilterAttribute.cs
       │     └── ValidateModelStateFilter.cs
       │
       ├── Models
       │     ├── Requests
       │     │     ├── Auth
       │     │     │     ├── RegisterRequest.cs
       │     │     │     ├── LoginRequest.cs
       │     │     │     └── RefreshTokenRequest.cs
       │     │     ├── Projects
       │     │     │     ├── CreateProjectRequest.cs
       │     │     │     ├── UpdateProjectRequest.cs
       │     │     │     └── ArchiveProjectRequest.cs
       │     │     ├── Tasks
       │     │     │     ├── CreateTaskRequest.cs
       │     │     │     ├── UpdateTaskRequest.cs
       │     │     │     ├── AssignTaskRequest.cs
       │     │     │     ├── UpdateTaskStatusRequest.cs
       │     │     │     └── CompleteTaskRequest.cs
       │     │     └── Teams
       │     │           ├── CreateTeamRequest.cs
       │     │           ├── AddTeamMemberRequest.cs
       │     │           └── ChangeTeamLeaderRequest.cs
       │     │
       │     └── Responses
       │           ├── ApiResponse.cs
       │           ├── ApiResponse{T}.cs
       │           ├── PaginatedResponse{T}.cs
       │           ├── ErrorResponse.cs
       │           └── ValidationErrorResponse.cs
       │
       ├── Extensions
       │     ├── ApplicationBuilderExtensions.cs
       │     ├── ServiceCollectionExtensions.cs
       │     └── ResultExtensions.cs
       │
       ├── Configuration
       │     ├── SwaggerConfiguration.cs
       │     ├── AuthenticationConfiguration.cs
       │     ├── CorsConfiguration.cs
       │     └── RateLimitConfiguration.cs
       │
       ├── appsettings.json
       ├── appsettings.Development.json
       ├── appsettings.Production.json
       ├── Program.cs
       └── WebApi.csproj

========================================
PACKAGE REFERENCES
========================================

Required NuGet packages (referenced in WebApi.csproj):

- Microsoft.AspNetCore.OpenApi (10.x or later)
- Swashbuckle.AspNetCore (6.x or later)
- Microsoft.AspNetCore.Authentication.JwtBearer (10.x or later)
- Microsoft.AspNetCore.Mvc.Versioning (5.x or later)
- Microsoft.AspNetCore.Mvc.Versioning.ApiExplorer (5.x or later)
- AspNetCoreRateLimit (5.x or later)
- Serilog.AspNetCore (8.x or later)
- Serilog.Sinks.Console
- Serilog.Sinks.File
- Serilog.Sinks.Seq (optional for centralized logging)
- Microsoft.Extensions.Diagnostics.HealthChecks.EntityFrameworkCore
- AspNetCore.HealthChecks.Npgsql
- AspNetCore.HealthChecks.Aws.S3
- MediatR (13.x or later) - already referenced via Application layer

Project References:
- Application project
- Infrastructure project

========================================
BASE CONTROLLER
========================================

**BaseApiController:**
```csharp
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[Produces("application/json")]
public abstract class BaseApiController : ControllerBase
{
    private ISender? _mediator;
    protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();

    protected ActionResult<ApiResponse<T>> Ok<T>(T data, string message = "Success")
    {
        return Ok(ApiResponse<T>.Success(data, message));
    }

    protected ActionResult<ApiResponse> Ok(string message = "Success")
    {
        return Ok(ApiResponse.Success(message));
    }

    protected ActionResult<ApiResponse> BadRequest(string message, IDictionary<string, string[]>? errors = null)
    {
        if (errors != null)
            return BadRequest(ValidationErrorResponse.Failure(message, errors));
        
        return BadRequest(ApiResponse.Failure(message));
    }

    protected ActionResult<ApiResponse> NotFound(string message = "Resource not found")
    {
        return NotFound(ApiResponse.Failure(message));
    }

    protected ActionResult HandleResult<T>(Result<T> result)
    {
        if (result.IsSuccess)
            return Ok(ApiResponse<T>.Success(result.Value!));

        return BadRequest(ApiResponse.Failure(result.Error.Message));
    }

    protected ActionResult HandleResult(Result result)
    {
        if (result.IsSuccess)
            return Ok(ApiResponse.Success());

        return BadRequest(ApiResponse.Failure(result.Error.Message));
    }
}
```

========================================
CONTROLLER REQUIREMENTS
========================================

**ProjectsController (V1):**
```csharp
[ApiVersion("1.0")]
[Authorize]
public class ProjectsController : BaseApiController
{
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<int>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> CreateProject(
        [FromBody] CreateProjectRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreateProjectCommand(
            request.Name,
            request.Description,
            request.OwnerId);

        var result = await Mediator.Send(command, cancellationToken);

        return HandleResult(result);
    }

    [HttpPut("{id}/archive")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> ArchiveProject(
        [FromRoute] int id,
        CancellationToken cancellationToken)
    {
        var command = new ArchiveProjectCommand(id);
        var result = await Mediator.Send(command, cancellationToken);
        return HandleResult(result);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> UpdateProject(
        [FromRoute] int id,
        [FromBody] UpdateProjectRequest request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateProjectCommand(
            id,
            request.Name,
            request.Description);

        var result = await Mediator.Send(command, cancellationToken);
        return HandleResult(result);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<ProjectDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetProjectById(
        [FromRoute] int id,
        CancellationToken cancellationToken)
    {
        var query = new GetProjectByIdQuery(id);
        var result = await Mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    [HttpGet]
    [ProducesResponseType(typeof(PaginatedResponse<ProjectDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult> GetAllProjects(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var query = new GetAllProjectsQuery(pageNumber, pageSize);
        var result = await Mediator.Send(query, cancellationToken);
        
        var response = new PaginatedResponse<ProjectDto>(
            result.Items,
            result.PageNumber,
            result.PageSize,
            result.TotalCount);
        
        return Ok(response);
    }

    [HttpGet("owner/{ownerId}")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<ProjectDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult> GetProjectsByOwner(
        [FromRoute] int ownerId,
        CancellationToken cancellationToken)
    {
        var query = new GetProjectsByOwnerQuery(ownerId);
        var result = await Mediator.Send(query, cancellationToken);
        return Ok(result);
    }
}
```

Follow same pattern for **TasksController** and **TeamsController** with their respective endpoints.

**AuthController:**
```csharp
[ApiVersion("1.0")]
[AllowAnonymous]
public class AuthController : BaseApiController
{
    [HttpPost("register")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Register(
        [FromBody] RegisterRequest request,
        CancellationToken cancellationToken)
    {
        var command = new RegisterUserCommand(
            request.Email,
            request.Password,
            request.FirstName,
            request.LastName);

        var result = await Mediator.Send(command, cancellationToken);
        return HandleResult(result);
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(ApiResponse<AuthResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Login(
        [FromBody] LoginRequest request,
        CancellationToken cancellationToken)
    {
        var command = new LoginCommand(request.Email, request.Password);
        var result = await Mediator.Send(command, cancellationToken);
        return HandleResult(result);
    }

    [HttpPost("refresh")]
    [ProducesResponseType(typeof(ApiResponse<AuthResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> RefreshToken(
        [FromBody] RefreshTokenRequest request,
        CancellationToken cancellationToken)
    {
        var command = new RefreshTokenCommand(request.RefreshToken);
        var result = await Mediator.Send(command, cancellationToken);
        return HandleResult(result);
    }

    [Authorize]
    [HttpPost("revoke")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult> RevokeToken(CancellationToken cancellationToken)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var command = new RevokeTokenCommand(int.Parse(userId!));
        var result = await Mediator.Send(command, cancellationToken);
        return HandleResult(result);
    }
}
```

========================================
REQUEST MODELS
========================================

All request models should have:
- Data annotations for basic validation (additional to FluentValidation)
- Required properties marked with [Required]
- String length constraints with [StringLength]
- Email validation with [EmailAddress]
- Example:
```csharp
public record CreateProjectRequest
{
    [Required(ErrorMessage = "Project name is required")]
    [StringLength(200, ErrorMessage = "Name cannot exceed 200 characters")]
    public string Name { get; init; } = string.Empty;

    [StringLength(2000, ErrorMessage = "Description cannot exceed 2000 characters")]
    public string? Description { get; init; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Owner ID must be greater than 0")]
    public int OwnerId { get; init; }
}

public record RegisterRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; init; } = string.Empty;

    [Required]
    [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 100 characters")]
    public string Password { get; init; } = string.Empty;

    [Required]
    [StringLength(50)]
    public string FirstName { get; init; } = string.Empty;

    [Required]
    [StringLength(50)]
    public string LastName { get; init; } = string.Empty;
}
```

========================================
RESPONSE MODELS
========================================

**ApiResponse (non-generic):**
```csharp
public record ApiResponse
{
    public bool Success { get; init; }
    public string Message { get; init; } = string.Empty;
    public DateTimeOffset Timestamp { get; init; }

    private ApiResponse(bool success, string message)
    {
        Success = success;
        Message = message;
        Timestamp = DateTimeOffset.UtcNow;
    }

    public static ApiResponse Success(string message = "Operation completed successfully")
        => new(true, message);

    public static ApiResponse Failure(string message)
        => new(false, message);
}
```

**ApiResponse<T> (generic):**
```csharp
public record ApiResponse<T> : ApiResponse
{
    public T? Data { get; init; }

    private ApiResponse(bool success, string message, T? data)
        : base(success, message)
    {
        Data = data;
    }

    public static ApiResponse<T> Success(T data, string message = "Operation completed successfully")
        => new(true, message, data);

    public static new ApiResponse<T> Failure(string message)
        => new(false, message, default);
}
```

**PaginatedResponse<T>:**
```csharp
public record PaginatedResponse<T>
{
    public IEnumerable<T> Items { get; init; }
    public int PageNumber { get; init; }
    public int PageSize { get; init; }
    public int TotalCount { get; init; }
    public int TotalPages { get; init; }
    public bool HasPreviousPage { get; init; }
    public bool HasNextPage { get; init; }

    public PaginatedResponse(IEnumerable<T> items, int pageNumber, int pageSize, int totalCount)
    {
        Items = items;
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalCount = totalCount;
        TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
        HasPreviousPage = pageNumber > 1;
        HasNextPage = pageNumber < TotalPages;
    }
}
```

**ErrorResponse:**
```csharp
public record ErrorResponse
{
    public string Type { get; init; } = string.Empty;
    public string Title { get; init; } = string.Empty;
    public int Status { get; init; }
    public string Detail { get; init; } = string.Empty;
    public string Instance { get; init; } = string.Empty;
    public string TraceId { get; init; } = string.Empty;
    public DateTimeOffset Timestamp { get; init; }

    public ErrorResponse(
        string type,
        string title,
        int status,
        string detail,
        string instance,
        string traceId)
    {
        Type = type;
        Title = title;
        Status = status;
        Detail = detail;
        Instance = instance;
        TraceId = traceId;
        Timestamp = DateTimeOffset.UtcNow;
    }
}
```

**ValidationErrorResponse:**
```csharp
public record ValidationErrorResponse : ApiResponse
{
    public IDictionary<string, string[]> Errors { get; init; }

    private ValidationErrorResponse(string message, IDictionary<string, string[]> errors)
        : base(false, message)
    {
        Errors = errors;
    }

    public static ValidationErrorResponse Failure(
        string message,
        IDictionary<string, string[]> errors)
        => new(message, errors);
}
```

========================================
MIDDLEWARE REQUIREMENTS
========================================

**ExceptionHandlingMiddleware:**
```csharp
public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var (status, title, detail) = exception switch
        {
            ValidationException validationEx => (
                StatusCodes.Status400BadRequest,
                "Validation Error",
                "One or more validation errors occurred"),
            
            NotFoundException notFoundEx => (
                StatusCodes.Status404NotFound,
                "Not Found",
                notFoundEx.Message),
            
            ForbiddenAccessException => (
                StatusCodes.Status403Forbidden,
                "Forbidden",
                "Access denied"),
            
            UnauthorizedAccessException => (
                StatusCodes.Status401Unauthorized,
                "Unauthorized",
                "Authentication required"),
            
            _ => (
                StatusCodes.Status500InternalServerError,
                "Internal Server Error",
                "An error occurred while processing your request")
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = status;

        var response = new ErrorResponse(
            type: $"https://httpstatuses.com/{status}",
            title: title,
            status: status,
            detail: detail,
            instance: context.Request.Path,
            traceId: context.TraceIdentifier);

        if (exception is ValidationException validationException)
        {
            var validationResponse = ValidationErrorResponse.Failure(
                "Validation failed",
                validationException.Errors);
            
            await context.Response.WriteAsJsonAsync(validationResponse);
            return;
        }

        await context.Response.WriteAsJsonAsync(response);
    }
}
```

**RequestLoggingMiddleware:**
```csharp
public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;

    public RequestLoggingMiddleware(
        RequestDelegate next,
        ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();

        _logger.LogInformation(
            "Incoming request: {Method} {Path} {QueryString}",
            context.Request.Method,
            context.Request.Path,
            context.Request.QueryString);

        await _next(context);

        stopwatch.Stop();

        _logger.LogInformation(
            "Completed request: {Method} {Path} - Status: {StatusCode} - Duration: {Duration}ms",
            context.Request.Method,
            context.Request.Path,
            context.Response.StatusCode,
            stopwatch.ElapsedMilliseconds);
    }
}
```

**CorrelationIdMiddleware:**
```csharp
public class CorrelationIdMiddleware
{
    private readonly RequestDelegate _next;
    private const string CorrelationIdHeader = "X-Correlation-ID";

    public CorrelationIdMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var correlationId = context.Request.Headers[CorrelationIdHeader].FirstOrDefault()
            ?? Guid.NewGuid().ToString();

        context.Items[CorrelationIdHeader] = correlationId;
        context.Response.Headers.Append(CorrelationIdHeader, correlationId);

        await _next(context);
    }
}
```

========================================
PROGRAM.CS STRUCTURE
========================================
```csharp
var builder = WebApplication.CreateBuilder(args);

// Serilog Configuration
builder.Host.UseSerilog((context, configuration) =>
{
    configuration
        .ReadFrom.Configuration(context.Configuration)
        .Enrich.FromLogContext()
        .Enrich.WithMachineName()
        .WriteTo.Console()
        .WriteTo.File(
            path: "logs/log-.txt",
            rollingInterval: RollingInterval.Day,
            retainedFileCountLimit: 30);
});

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Application Layer
builder.Services.AddApplication();

// Infrastructure Layer
builder.Services.AddInfrastructure(builder.Configuration);

// API Configuration
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
    options.ApiVersionReader = new UrlSegmentApiVersionReader();
});

builder.Services.AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

// Swagger Configuration
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Task Management API",
        Version = "v1",
        Description = "Task Management System API",
        Contact = new OpenApiContact
        {
            Name = "Support Team",
            Email = "support@taskmanagement.com"
        }
    });

    // JWT Authentication for Swagger
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter JWT token"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });

    // XML Comments
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath);
    }
});

// Authentication & Authorization
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)),
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });

    options.AddPolicy("Production", policy =>
    {
        policy.WithOrigins(builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>()!)
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

// Rate Limiting
builder.Services.AddMemoryCache();
builder.Services.Configure<IpRateLimitOptions>(options =>
{
    options.GeneralRules = new List<RateLimitRule>
    {
        new()
        {
            Endpoint = "*",
            Period = "1m",
            Limit = 60
        }
    };
});
builder.Services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
builder.Services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
builder.Services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();

// Health Checks
builder.Services.AddHealthChecks()
    .AddDbContextCheck<ApplicationDbContext>()
    .AddNpgSql(builder.Configuration.GetConnectionString("DefaultConnection")!)
    .AddCheck("self", () => HealthCheckResult.Healthy());

var app = builder.Build();

// Configure the HTTP request pipeline
app.UseMiddleware<CorrelationIdMiddleware>();
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseMiddleware<RequestLoggingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Task Management API V1");
        options.RoutePrefix = string.Empty; // Swagger at root
    });
    
    // Initialize database in development
    using var scope = app.Services.CreateScope();
    var initializer = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitializer>();
    await initializer.InitializeAsync();
    await initializer.SeedAsync();
}

app.UseHttpsRedirection();

app.UseCors(app.Environment.IsDevelopment() ? "AllowAll" : "Production");

app.UseIpRateLimiting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapHealthChecks("/health");
app.MapHealthChecks("/health/ready", new HealthCheckOptions
{
    Predicate = check => check.Tags.Contains("ready")
});
app.MapHealthChecks("/health/live", new HealthCheckOptions
{
    Predicate = _ => false
});

app.UseSerilogRequestLogging();

app.Run();

// Make the implicit Program class public for testing
public partial class Program { }
```

========================================
APPSETTINGS.JSON
========================================
```json
{
  "Serilog": {
    "MinimumLevel": {
      "Default": "Information",
      "Override": {
        "Microsoft": "Warning",
        "Microsoft.Hosting.Lifetime": "Information",
        "Microsoft.EntityFrameworkCore": "Warning"
      }
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=TaskManagement;Username=postgres;Password=postgres"
  },
  "DatabaseSettings": {
    "EnableSensitiveDataLogging": false
  },
  "Jwt": {
    "Key": "YourSuperSecretKeyThatIsAtLeast32CharactersLong!",
    "Issuer": "TaskManagementApi",
    "Audience": "TaskManagementClient",
    "ExpiryMinutes": 60,
    "RefreshTokenExpiryDays": 7
  },
  "Cors": {
    "AllowedOrigins": [
      "http://localhost:3000",
      "https://localhost:3000"
    ]
  },
  "IpRateLimiting": {
    "EnableEndpointRateLimiting": true,
    "StackBlockedRequests": false,
    "HttpStatusCode": 429,
    "GeneralRules": [
      {
        "Endpoint": "*",
        "Period": "1m",
        "Limit": 60
      }
    ]
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

**appsettings.Development.json:**
```json
{
  "Serilog": {
    "MinimumLevel": {
      "Default": "Debug"
    }
  },
  "DatabaseSettings": {
    "EnableSensitiveDataLogging": true
  }
}
```

**appsettings.Production.json:**
```json
{
  "Serilog": {
    "MinimumLevel": {
      "Default": "Warning"
    }
  },
  "ConnectionStrings": {
    "DefaultConnection": "Use AWS Secrets Manager"
  },
  "Jwt": {
    "Key": "Use AWS Secrets Manager"
  }
}
```

========================================
EXTENSIONS
========================================

**ResultExtensions:**
```csharp
public static class ResultExtensions
{
    public static ActionResult ToActionResult<T>(this Result<T> result)
    {
        if (result.IsSuccess)
            return new OkObjectResult(ApiResponse<T>.Success(result.Value!));

        return new BadRequestObjectResult(ApiResponse.Failure(result.Error.Message));
    }

    public static ActionResult ToActionResult(this Result result)
    {
        if (result.IsSuccess)
            return new OkObjectResult(ApiResponse.Success());

        return new BadRequestObjectResult(ApiResponse.Failure(result.Error.Message));
    }
}
```

========================================
IMPLEMENTATION CONSTRAINTS
========================================

- All controllers inherit from BaseApiController
- Use [ApiVersion("1.0")] on all controllers
- Use [Authorize] on protected endpoints
- Use [AllowAnonymous] on public endpoints (auth)
- All endpoints return ActionResult or ActionResult<T>
- Use ProducesResponseType for Swagger documentation
- Use XML comments for all public API methods
- Request validation via Data Annotations + FluentValidation
- Response models always wrapped in ApiResponse<T>
- Pagination for all list endpoints
- CancellationToken in all async methods
- FromBody, FromRoute, FromQuery attributes explicit
- HTTP verbs explicit: [HttpGet], [HttpPost], etc.
- RESTful conventions (nouns, not verbs in URLs)
- Consistent error responses (RFC 7807 Problem Details)
- Correlation ID in all requests
- Request/response logging
- Rate limiting per IP
- CORS properly configured per environment
- JWT authentication with refresh token support
- Role-based authorization where needed
- Health checks for all dependencies
- Graceful shutdown support
- Proper DI container usage
- No business logic in controllers
- Thin controllers (only orchestration)
- MediatR for all command/query execution

========================================
EXPECTED OUTPUT FILES
========================================

Total: ~45 files

**Controllers (5 files):**
1. src/WebApi/Controllers/BaseApiController.cs
2. src/WebApi/Controllers/V1/ProjectsController.cs
3. src/WebApi/Controllers/V1/TasksController.cs
4. src/WebApi/Controllers/V1/TeamsController.cs
5. src/WebApi/Controllers/V1/AuthController.cs

**Middleware (3 files):**
6. src/WebApi/Middleware/ExceptionHandlingMiddleware.cs
7. src/WebApi/Middleware/RequestLoggingMiddleware.cs
8. src/WebApi/Middleware/CorrelationIdMiddleware.cs

**Filters (2 files):**
9. src/WebApi/Filters/ApiExceptionFilterAttribute.cs
10. src/WebApi/Filters/ValidateModelStateFilter.cs

**Models/Requests (14 files):**
11-13. Auth requests (3 files)
14-16. Project requests (3 files)
17-21. Task requests (5 files)
22-24. Team requests (3 files)

**Models/Responses (5 files):**
25. src/WebApi/Models/Responses/ApiResponse.cs
26. src/WebApi/Models/Responses/ApiResponse{T}.cs
27. src/WebApi/Models/Responses/PaginatedResponse{T}.cs
28. src/WebApi/Models/Responses/ErrorResponse.cs
29. src/WebApi/Models/Responses/ValidationErrorResponse.cs

**Extensions (3 files):**
30. src/WebApi/Extensions/ApplicationBuilderExtensions.cs
31. src/WebApi/Extensions/ServiceCollectionExtensions.cs
32. src/WebApi/Extensions/ResultExtensions.cs

**Configuration (4 files - optional):**
33. src/WebApi/Configuration/SwaggerConfiguration.cs
34. src/WebApi/Configuration/AuthenticationConfiguration.cs
35. src/WebApi/Configuration/CorsConfiguration.cs
36. src/WebApi/Configuration/RateLimitConfiguration.cs

**Root Files (4 files):**
37. src/WebApi/Program.cs
38. src/WebApi/appsettings.json
39. src/WebApi/appsettings.Development.json
40. src/WebApi/appsettings.Production.json
41. src/WebApi/WebApi.csproj

========================================
OUTPUT FORMAT
========================================

Return full C# code for all files.

Each file must be separated with:

// File: src/WebApi/...

Use proper namespaces following the folder structure pattern:
- TaskManagement.WebApi
- TaskManagement.WebApi.Controllers.V1
- TaskManagement.WebApi.Middleware
- TaskManagement.WebApi.Models.Requests
- TaskManagement.WebApi.Models.Responses
- TaskManagement.WebApi.Extensions
- TaskManagement.WebApi.Configuration

Include all necessary using statements.
Reference Application and Infrastructure namespaces where needed.

For appsettings.json files, use JSON format with proper indentation.

Only return code. No explanations. No markdown. No comments outside code.

Begin output now.