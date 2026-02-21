# Step 6: Final Verification, CI & Docker

After all layers are implemented, execute these final steps.

## Phase 1: Architecture Verification

Verify the dependency rule is not violated. Check each .csproj:

```bash
echo "=== Domain.csproj (must have ZERO PackageReference) ==="
cat src/Domain/TaskManagement.Domain.csproj

echo "=== Application.csproj (must reference ONLY Domain) ==="
cat src/Application/TaskManagement.Application.csproj

echo "=== Infrastructure.csproj (must reference Application, NOT Domain directly) ==="
cat src/Infrastructure/TaskManagement.Infrastructure.csproj
```

Fix any violations found.

## Phase 2: Build & Test

```bash
dotnet build --configuration Release
dotnet test --configuration Release --no-build --verbosity normal
```

Both must pass. Fix anything that fails.

## Phase 3: GitHub Actions CI

Create `.github/workflows/ci.yml`:

```yaml
name: CI

on:
  push:
    branches: [main, develop]
  pull_request:
    branches: [main, develop]

env:
  DOTNET_VERSION: '10.0.x'

jobs:
  build-and-test:
    runs-on: ubuntu-latest

    services:
      postgres:
        image: postgres:16
        env:
          POSTGRES_USER: postgres
          POSTGRES_PASSWORD: postgres
          POSTGRES_DB: taskmanagement_test
        ports:
          - 5432:5432
        options: >-
          --health-cmd pg_isready
          --health-interval 10s
          --health-timeout 5s
          --health-retries 5

    steps:
      - uses: actions/checkout@v4

      - name: Setup .NET
        uses: actions/setup-dotnet@v4
        with:
          dotnet-version: ${{ env.DOTNET_VERSION }}

      - name: Restore dependencies
        run: dotnet restore

      - name: Build
        run: dotnet build --configuration Release --no-restore

      - name: Unit Tests
        run: |
          dotnet test tests/Domain.UnitTests --configuration Release --no-build --verbosity normal
          dotnet test tests/Application.UnitTests --configuration Release --no-build --verbosity normal

      - name: Integration Tests
        run: |
          dotnet test tests/Infrastructure.IntegrationTests --configuration Release --no-build --verbosity normal
          dotnet test tests/Api.IntegrationTests --configuration Release --no-build --verbosity normal
        env:
          ConnectionStrings__DefaultConnection: "Host=localhost;Database=taskmanagement_test;Username=postgres;Password=postgres"
```

## Phase 4: PR Template

Create `.github/pull_request_template.md`:

```markdown
## What does this PR do?

## Type of change
- [ ] Bug fix
- [ ] New feature
- [ ] Refactoring
- [ ] Documentation

## Architecture Checklist
- [ ] Domain has no external dependencies
- [ ] Business logic is in Domain entities, not handlers
- [ ] New commands have FluentValidation validators
- [ ] Architecture tests pass
- [ ] No direct DbContext usage in Application layer
- [ ] DateTimeOffset used (not DateTime)
- [ ] EF configs use Fluent API only (no Data Annotations)

## Testing
- [ ] Unit tests added/updated
- [ ] Integration tests added/updated
- [ ] All tests pass locally
```

## Phase 5: Dockerfile

Create `Dockerfile` in solution root:

```dockerfile
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY *.sln .
COPY Directory.Build.props .
COPY src/Domain/*.csproj src/Domain/
COPY src/Application/*.csproj src/Application/
COPY src/Infrastructure/*.csproj src/Infrastructure/
COPY src/WebApi/*.csproj src/WebApi/

RUN dotnet restore

COPY src/ src/
RUN dotnet publish src/WebApi -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "TaskManagement.WebApi.dll"]
```

## Phase 6: docker-compose

Create `docker-compose.yml`:

```yaml
services:
  api:
    build: .
    ports:
      - "8080:8080"
    environment:
      - ConnectionStrings__DefaultConnection=Host=db;Database=TaskManagement;Username=postgres;Password=postgres
      - ASPNETCORE_ENVIRONMENT=Development
    depends_on:
      db:
        condition: service_healthy

  db:
    image: postgres:16
    ports:
      - "5432:5432"
    environment:
      POSTGRES_USER: postgres
      POSTGRES_PASSWORD: postgres
      POSTGRES_DB: TaskManagement
    volumes:
      - pgdata:/var/lib/postgresql/data
    healthcheck:
      test: ["CMD-SHELL", "pg_isready -U postgres"]
      interval: 5s
      timeout: 5s
      retries: 5

volumes:
  pgdata:
```

## Phase 7: Final Build Check

```bash
dotnet build
dotnet test
```

Everything must pass.
