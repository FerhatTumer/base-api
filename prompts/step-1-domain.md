# Domain Layer - Clean Architecture Prompt

You are a senior .NET software architect.

Generate a complete Domain Layer using:

- Clean Architecture
- Domain-Driven Design (DDD)
- .NET 10
- C#
- EF Core will be used in Infrastructure (but DO NOT reference EF in Domain)

The output must ONLY contain the Domain Layer code.
No explanations. No comments outside code.
Follow the folder structure and rules exactly.

========================================
BUSINESS DOMAIN
========================================

This is a Task Management System with the following aggregates:

**Aggregate: Project**
Root Entity: Project
- Properties: Name (string, required, max 200), Description (string, optional, max 2000), OwnerId (int, required), Status (ProjectStatus enum)
- Has many TaskItems (child entities within aggregate)
- Business Rules:
  * Name cannot be empty or whitespace
  * Cannot add tasks to an archived project
  * Cannot archive a project with active tasks
  * Owner cannot be changed after creation
  * Maximum 100 tasks per project

**Aggregate: Team**
Root Entity: Team
- Properties: Name (string, required, max 100), Description (string, optional, max 1000), LeaderId (int, required)
- Has many TeamMembers (child entities within aggregate)
- Business Rules:
  * Name must be unique (validated at application layer)
  * Leader must be a team member
  * Maximum 50 members per team
  * Cannot remove the team leader without assigning a new one

**Child Entity: TaskItem**
Belongs to Project aggregate
- Properties: Title (string, required, max 200), Description (string, optional, max 2000), Priority (Priority enum), Status (TaskStatus enum), DueDate (DateTimeOffset?), AssigneeId (int?), ProjectId (int, required), EstimatedHours (decimal?)
- Business Rules:
  * Title cannot be empty
  * DueDate must be in the future when set
  * Cannot mark as completed without an assignee
  * EstimatedHours must be positive when set
  * Status progression: Todo -> InProgress -> Done (cannot skip states)

**Child Entity: TeamMember**
Belongs to Team aggregate
- Properties: UserId (int, required), Role (TeamRole enum), JoinedAt (DateTimeOffset)
- Business Rules:
  * Same user cannot be added twice to the same team
  * Role can only be changed by team leader

**Value Objects:**
- Money: Amount (decimal), Currency (Currency enum)
- DateRange: StartDate (DateTimeOffset), EndDate (DateTimeOffset)
  * EndDate must be after StartDate
- Email: Value (string)
  * Must be valid email format

**Enums:**
- ProjectStatus: Active, Archived
- TaskStatus: Todo, InProgress, Done, Cancelled
- Priority: Low, Medium, High, Critical
- TeamRole: Member, Leader
- Currency: USD, EUR, TRY, GBP

**Domain Events:**
- ProjectCreatedEvent: ProjectId, Name, CreatedAt
- ProjectArchivedEvent: ProjectId, ArchivedAt
- TaskCreatedEvent: TaskId, ProjectId, Title, CreatedAt
- TaskAssignedEvent: TaskId, AssigneeId, AssignedAt
- TaskCompletedEvent: TaskId, CompletedAt
- TeamMemberAddedEvent: TeamId, UserId, Role, AddedAt
- TeamLeaderChangedEvent: TeamId, OldLeaderId, NewLeaderId, ChangedAt

========================================
ARCHITECTURE RULES
========================================

- Domain layer must be inside: src/Domain
- No external libraries allowed (except System.* BCL)
- No EF Core attributes or references
- System.Linq.Expressions is allowed (part of .NET BCL) for IRepository
- Pure domain model only
- All business rules must be enforced inside entities
- Entities must protect invariants
- Collections must be encapsulated (private list + IReadOnlyCollection exposure)
- Use proper constructors and methods to enforce rules
- Throw DomainException for business rule violations
- Do NOT implement IEquatable manually unless required for ValueObject
- Do NOT manually assign Id values (EF Core will set them)
- Prefer int as Id type in implementations
- ALL date types must be DateTimeOffset
- Use DateTimeOffset.UtcNow for timestamps
- ALL async methods must accept CancellationToken
- Soft deletion must be supported via IsDeleted
- ValueObjects must be implemented as sealed classes inheriting from ValueObject base class
- Do NOT use C# records for ValueObjects
- Cross-aggregate references must use Id only (e.g., ProjectId, not Project navigation)
- Child entities within an aggregate may have navigation properties to their parent aggregate root
- No ICollection<T> exposed as public on entities
- Aggregates must expose static factory methods: Create(...) for instantiation
- Direct instantiation via public constructor must be prevented
- All entities must include a protected parameterless constructor to support ORM hydration
- This protected constructor must not enforce business rules
- Do NOT implement Specification Pattern in Domain
- Query filtering is handled via Expression predicates in IRepository

========================================
FOLDER STRUCTURE
========================================

src
 └── Domain
       ├── Common
       │     ├── Entity.cs
       │     ├── AggregateRoot.cs
       │     ├── ValueObject.cs
       │     ├── DomainEvent.cs
       │     ├── IRepository.cs
       │     ├── IUnitOfWork.cs
       │     └── DomainException.cs
       │
       ├── Enums
       │     ├── ProjectStatus.cs
       │     ├── TaskStatus.cs
       │     ├── Priority.cs
       │     ├── TeamRole.cs
       │     └── Currency.cs
       │
       └── Aggregates
             ├── ProjectAggregate
             │     ├── Project.cs
             │     ├── TaskItem.cs
             │     ├── IProjectRepository.cs
             │     ├── Events
             │     │     ├── ProjectCreatedEvent.cs
             │     │     ├── ProjectArchivedEvent.cs
             │     │     ├── TaskCreatedEvent.cs
             │     │     ├── TaskAssignedEvent.cs
             │     │     └── TaskCompletedEvent.cs
             │     └── ValueObjects
             │           └── DateRange.cs
             │
             └── TeamAggregate
                   ├── Team.cs
                   ├── TeamMember.cs
                   ├── ITeamRepository.cs
                   ├── Events
                   │     ├── TeamMemberAddedEvent.cs
                   │     └── TeamLeaderChangedEvent.cs
                   └── ValueObjects
                         ├── Money.cs
                         └── Email.cs

========================================
BASE CLASS REQUIREMENTS
========================================

----------------------------------------
Entity<TId>
----------------------------------------

Properties:
- TId Id { get; protected set; }
- DateTimeOffset CreatedAt { get; protected set; }
- DateTimeOffset? UpdatedAt { get; protected set; }
- bool IsDeleted { get; protected set; }

Protected Constructor:
- CreatedAt = DateTimeOffset.UtcNow
- IsDeleted = false

Methods:
- protected void SetUpdated() → UpdatedAt = DateTimeOffset.UtcNow
- public virtual void SoftDelete()
  * If already deleted → throw DomainException
  * Set IsDeleted = true
  * Call SetUpdated()

Must include protected parameterless constructor for ORM.
Do NOT implement equality logic in Entity base class.

----------------------------------------
AggregateRoot<TId>
----------------------------------------

Inherits from Entity<TId>

Additional Members:
- private readonly List<DomainEvent> _domainEvents = new();
- public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents.AsReadOnly();

Methods:
- protected void AddDomainEvent(DomainEvent domainEvent)
- public void ClearDomainEvents()

DomainEvents must NOT be modifiable from outside.
Must include protected parameterless constructor for ORM.

----------------------------------------
ValueObject
----------------------------------------

Abstract base class.

Requirements:
- Must implement equality comparison based on atomic values
- Override Equals(object? obj)
- Override GetHashCode()
- Implement == and != operators
- Provide protected abstract IEnumerable<object> GetAtomicValues()

ValueObjects must be immutable (init-only or private setters).

Template:
```csharp
public abstract class ValueObject
{
    protected abstract IEnumerable<object> GetAtomicValues();
    
    public override bool Equals(object? obj)
    {
        if (obj == null || obj.GetType() != GetType())
            return false;
            
        var other = (ValueObject)obj;
        return GetAtomicValues().SequenceEqual(other.GetAtomicValues());
    }
    
    public override int GetHashCode()
    {
        return GetAtomicValues()
            .Select(x => x?.GetHashCode() ?? 0)
            .Aggregate((x, y) => x ^ y);
    }
    
    public static bool operator ==(ValueObject? left, ValueObject? right)
    {
        if (left is null && right is null) return true;
        if (left is null || right is null) return false;
        return left.Equals(right);
    }
    
    public static bool operator !=(ValueObject? left, ValueObject? right)
    {
        return !(left == right);
    }
}
```

----------------------------------------
DomainEvent
----------------------------------------

Abstract base class.

Properties:
- public DateTimeOffset OccurredOn { get; }

Constructor:
- OccurredOn = DateTimeOffset.UtcNow

No external dependencies.
No MediatR reference.
Pure domain abstraction only.

----------------------------------------
IRepository<T, TId>
----------------------------------------

Generic repository interface for aggregate roots only.

Constraint: where T : AggregateRoot<TId>

Methods:
- Task AddAsync(T entity, CancellationToken cancellationToken);
- void Update(T entity);
- void Remove(T entity);
- Task<T?> GetByIdAsync(TId id, CancellationToken cancellationToken);
- Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken);
- Task<IEnumerable<T>> WhereAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken);

Note: System.Linq.Expressions is part of .NET BCL and allowed in Domain.

----------------------------------------
IUnitOfWork
----------------------------------------

Methods:
- Task<int> SaveChangesAsync(CancellationToken cancellationToken);
- Task BeginTransactionAsync(CancellationToken cancellationToken);
- Task CommitTransactionAsync(CancellationToken cancellationToken);
- Task RollbackTransactionAsync(CancellationToken cancellationToken);

----------------------------------------
DomainException
----------------------------------------

Custom exception for business rule violations.

Inherits from Exception.

Constructor:
- DomainException(string message) : base(message)

========================================
IMPLEMENTATION CONSTRAINTS
========================================

- Use private setters for properties
- Protect collections with private list + IReadOnlyCollection pattern
- Use static factory methods (Create) for aggregate creation
- Private constructors for business logic
- Protected parameterless constructors for ORM
- Use decimal for monetary values
- Use DateTimeOffset everywhere for dates
- No persistence logic in domain
- No infrastructure references
- Cross-aggregate references via Id only
- Do not manually assign Id values (EF Core handles this)
- Prevent operations on deleted entities (check IsDeleted)
- DomainEvents must only be added inside aggregates
- Validate all inputs in constructors and methods
- Throw DomainException with descriptive messages for violations
- Use const for magic numbers (e.g., const int MaxTasksPerProject = 100)
- Child entities must have navigation property to parent aggregate root
- Enum properties must validate against defined enum values

========================================
EXPECTED OUTPUT FILES
========================================

1. src/Domain/Common/Entity.cs
2. src/Domain/Common/AggregateRoot.cs
3. src/Domain/Common/ValueObject.cs
4. src/Domain/Common/DomainEvent.cs
5. src/Domain/Common/IRepository.cs
6. src/Domain/Common/IUnitOfWork.cs
7. src/Domain/Common/DomainException.cs
8. src/Domain/Enums/ProjectStatus.cs
9. src/Domain/Enums/TaskStatus.cs
10. src/Domain/Enums/Priority.cs
11. src/Domain/Enums/TeamRole.cs
12. src/Domain/Enums/Currency.cs
13. src/Domain/Aggregates/ProjectAggregate/Project.cs
14. src/Domain/Aggregates/ProjectAggregate/TaskItem.cs
15. src/Domain/Aggregates/ProjectAggregate/IProjectRepository.cs
16. src/Domain/Aggregates/ProjectAggregate/Events/ProjectCreatedEvent.cs
17. src/Domain/Aggregates/ProjectAggregate/Events/ProjectArchivedEvent.cs
18. src/Domain/Aggregates/ProjectAggregate/Events/TaskCreatedEvent.cs
19. src/Domain/Aggregates/ProjectAggregate/Events/TaskAssignedEvent.cs
20. src/Domain/Aggregates/ProjectAggregate/Events/TaskCompletedEvent.cs
21. src/Domain/Aggregates/ProjectAggregate/ValueObjects/DateRange.cs
22. src/Domain/Aggregates/TeamAggregate/Team.cs
23. src/Domain/Aggregates/TeamAggregate/TeamMember.cs
24. src/Domain/Aggregates/TeamAggregate/ITeamRepository.cs
25. src/Domain/Aggregates/TeamAggregate/Events/TeamMemberAddedEvent.cs
26. src/Domain/Aggregates/TeamAggregate/Events/TeamLeaderChangedEvent.cs
27. src/Domain/Aggregates/TeamAggregate/ValueObjects/Money.cs
28. src/Domain/Aggregates/TeamAggregate/ValueObjects/Email.cs

========================================
OUTPUT FORMAT
========================================

Return full C# code for all 28 files listed above.

Each file must be separated with:

// File: src/Domain/...

Use proper namespaces following the folder structure.

Only return code. No explanations. No markdown. No comments outside code.

Begin output now.