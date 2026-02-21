# TaskManagement — DDD + Clean Architecture (.NET 10)

## Solution Structure
```
TaskManagement.sln
src/
├── Domain/               # Entities, Value Objects, Aggregates, Domain Events
├── Application/          # CQRS Commands/Queries, MediatR, FluentValidation
├── Infrastructure/       # EF Core (PostgreSQL), AWS Services, Repositories
└── WebApi/               # Controllers, Middleware, Composition Root
tests/
├── Domain.UnitTests/
├── Application.UnitTests/
├── Infrastructure.IntegrationTests/
└── Api.IntegrationTests/
```

## Dependency Rule — NEVER VIOLATE
```
Domain            → NOTHING (zero NuGet, zero project refs, only .NET BCL)
Application       → Domain only (+ MediatR, FluentValidation, AutoMapper)
Infrastructure    → Application + Domain (+ EF Core, AWS SDK, Npgsql)
WebApi            → Application + Infrastructure (Composition Root)
```

Before writing ANY code, identify which layer it belongs to. If you are about to add a `using` or reference that violates the dependency rule above, STOP and restructure.

## Domain Layer Rules
- ZERO external dependencies. No NuGet packages. No project references. Only .NET BCL.
- System.Linq.Expressions is allowed (part of BCL) for IRepository.
- Rich domain models: business logic lives IN entities, not in services or handlers.
- Entities inherit from `Entity<TId>`, aggregates from `AggregateRoot<TId>`.
- Use `int` as Id type. EF Core auto-generates Ids — never assign manually.
- Constructors are `private`. Use static factory methods: `Entity.Create(...)`.
- Protected parameterless constructors for ORM hydration (no business rules in them).
- No public setters. State changes through behavior methods only.
- Collections: private `List<T>` + public `IReadOnlyCollection<T>` exposure.
- Cross-aggregate references via Id only (e.g., `ProjectId`, not `Project` navigation).
- Child entities within aggregate may have navigation to parent aggregate root.
- Domain events raised via `AggregateRoot.AddDomainEvent()`.
- Throw `DomainException` for business rule violations — NOT Result pattern in Domain.
- ValueObjects: sealed classes inheriting from `ValueObject` base. NOT C# records.
- All date types must be `DateTimeOffset`. Use `DateTimeOffset.UtcNow`.
- Soft delete via `IsDeleted` property on `Entity<TId>`.
- Use `const` for magic numbers (e.g., `const int MaxTasksPerProject = 100`).
- Validate enum values in constructors/methods.
- No Specification Pattern in Domain — use Expression predicates in IRepository.

## Application Layer Rules
- CQRS with MediatR. Commands modify → `Result<T>`. Queries read → DTOs.
- Feature-based folders: `Projects/Commands/CreateProject/`, `Projects/Queries/GetProjectById/`
- Each command folder: Command.cs, CommandHandler.cs, CommandValidator.cs
- FluentValidation validator required for every Command AND Query with parameters.
- AutoMapper for Domain → DTO mapping via `MappingProfile.cs`.
- Pipeline behaviors (IPipelineBehavior): ValidationBehavior, LoggingBehavior, TransactionBehavior.
- Domain events handled via `INotificationHandler<T>`.
- Use `ICurrentUserService`, `IDateTimeProvider` abstractions (not static DateTime).
- Throw `NotFoundException` when entity not found, `ForbiddenAccessException` for auth.
- Pagination defaults: PageNumber=1, PageSize=10, MaxPageSize=100.
- No EF Core references. No direct database access.
- All handlers async with CancellationToken.

## Infrastructure Layer Rules
- EF Core with PostgreSQL (Npgsql).
- Fluent API only — NO Data Annotations in Domain.
- Separate `IEntityTypeConfiguration<T>` per entity.
- Enum to string conversion. DateTimeOffset for all dates.
- Shadow property `RowVersion` (xmin for PostgreSQL) as concurrency token.
- Global query filter: `!IsDeleted` on all entities.
- No cascade delete — Restrict/NoAction only.
- Interceptors: AuditableEntityInterceptor, SoftDeleteInterceptor, DispatchDomainEventsInterceptor.
- Domain event dispatching via interceptor (NOT in SaveChangesAsync override).
- Index naming: `IX_{Table}_{Column}`.
- FK naming: `FK_{Table}_{ReferencedTable}_{Column}`.
- Retry on transient failure (maxRetryCount: 3).
- Use AsNoTracking for read-only queries.

## WebApi Layer Rules
- Controller-based API. All controllers inherit `BaseApiController`.
- Thin controllers: only call MediatR, no business logic.
- API versioning via URL: `api/v{version:apiVersion}/[controller]`.
- JWT Bearer authentication. Role-based authorization.
- Global exception handling middleware → ProblemDetails (RFC 7807).
- Consistent response: `ApiResponse<T>` wrapper.
- Correlation ID middleware on all requests.
- Rate limiting, CORS, Health Checks.
- Serilog for structured logging.
- `[ProducesResponseType]` on all endpoints for Swagger.
- CancellationToken on all async endpoints.

## Naming Conventions
| Type | Pattern | Example |
|------|---------|---------|
| Command | `{Verb}{Entity}Command` | `CreateProjectCommand` |
| Query | `Get{Entity}By{Criteria}Query` | `GetProjectByIdQuery` |
| Handler | `{RequestName}Handler` | `CreateProjectCommandHandler` |
| Validator | `{RequestName}Validator` | `CreateProjectCommandValidator` |
| Domain Event | `{Entity}{PastVerb}Event` | `ProjectCreatedEvent` |
| Value Object | Descriptive noun (sealed class) | `Money`, `Email`, `DateRange` |
| Repository | `I{Entity}Repository` | `IProjectRepository` |
| EF Config | `{Entity}Configuration` | `ProjectConfiguration` |
| DTO | `{Entity}Dto`, `{Entity}DetailDto` | `ProjectDto`, `ProjectDetailDto` |
| Request | `{Action}{Entity}Request` | `CreateProjectRequest` |

## Code Style
- C# 13 / .NET 10. File-scoped namespaces.
- `sealed` on classes where appropriate.
- `record` for Commands, Queries, DTOs. NOT for ValueObjects or Entities.
- Nullable reference types enabled everywhere.
- No `#region` blocks. One class per file.
- Use `const` for string literals and magic numbers.
- AAA pattern (Arrange-Act-Assert) in tests.
- Test naming: `MethodName_Scenario_ExpectedResult`.

## Review Checklist — Apply to EVERY code generation
- [ ] Correct layer? Dependencies flow inward only?
- [ ] Domain: private constructor + factory method?
- [ ] Domain: no public setters, encapsulated collections?
- [ ] Domain: DomainException for rule violations?
- [ ] Application: Command has validator?
- [ ] Application: Handler uses repository interface, not DbContext?
- [ ] Infrastructure: Fluent API config, no Data Annotations?
- [ ] WebApi: Thin controller, MediatR only?
- [ ] Naming conventions followed?
- [ ] DateTimeOffset used (not DateTime)?

## What NEVER To Do
- ❌ Add NuGet packages to Domain project
- ❌ Reference Infrastructure from Application
- ❌ Use public setters on domain entities
- ❌ Use `new Entity()` — always factory methods
- ❌ Throw generic exceptions for business rules (use DomainException)
- ❌ Use C# records for ValueObjects
- ❌ Use Data Annotations in Domain
- ❌ Cascade delete in EF configurations
- ❌ Put business logic in Controllers or Handlers
- ❌ Skip FluentValidation on commands
- ❌ Use DateTime instead of DateTimeOffset
- ❌ Manually assign entity Ids
- ❌ Use static DateTime (use IDateTimeProvider)
- ❌ Violate dependency rule (Domain → Application → Infrastructure → WebApi)