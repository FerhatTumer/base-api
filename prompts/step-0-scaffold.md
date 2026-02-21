# Step 0: Solution Scaffold

Create the TaskManagement solution with all projects, references, and packages.
Execute ALL commands below sequentially. Do NOT ask for confirmation between steps.

## Phase 1: Solution & Projects

```bash
dotnet new sln -n TaskManagement

# Source projects
dotnet new classlib -n TaskManagement.Domain -o src/Domain
dotnet new classlib -n TaskManagement.Application -o src/Application
dotnet new classlib -n TaskManagement.Infrastructure -o src/Infrastructure
dotnet new webapi -n TaskManagement.WebApi -o src/WebApi --use-controllers

# Test projects
dotnet new xunit -n TaskManagement.Domain.UnitTests -o tests/Domain.UnitTests
dotnet new xunit -n TaskManagement.Application.UnitTests -o tests/Application.UnitTests
dotnet new xunit -n TaskManagement.Infrastructure.IntegrationTests -o tests/Infrastructure.IntegrationTests
dotnet new xunit -n TaskManagement.Api.IntegrationTests -o tests/Api.IntegrationTests

# Add all to solution
dotnet sln add src/Domain
dotnet sln add src/Application
dotnet sln add src/Infrastructure
dotnet sln add src/WebApi
dotnet sln add tests/Domain.UnitTests
dotnet sln add tests/Application.UnitTests
dotnet sln add tests/Infrastructure.IntegrationTests
dotnet sln add tests/Api.IntegrationTests
```

## Phase 2: Project References (Dependency Rule)

```bash
# Application → Domain
dotnet add src/Application reference src/Domain

# Infrastructure → Application (transitively gets Domain)
dotnet add src/Infrastructure reference src/Application

# WebApi → Application + Infrastructure (Composition Root)
dotnet add src/WebApi reference src/Application
dotnet add src/WebApi reference src/Infrastructure

# Test references
dotnet add tests/Domain.UnitTests reference src/Domain
dotnet add tests/Application.UnitTests reference src/Application
dotnet add tests/Application.UnitTests reference src/Domain
dotnet add tests/Infrastructure.IntegrationTests reference src/Infrastructure
dotnet add tests/Infrastructure.IntegrationTests reference src/Application
dotnet add tests/Infrastructure.IntegrationTests reference src/Domain
dotnet add tests/Api.IntegrationTests reference src/WebApi
dotnet add tests/Api.IntegrationTests reference src/Application
dotnet add tests/Api.IntegrationTests reference src/Infrastructure
```

## Phase 3: NuGet Packages

IMPORTANT: Do NOT add any packages to TaskManagement.Domain. It must have ZERO PackageReference items.

```bash
# Application
dotnet add src/Application package MediatR
dotnet add src/Application package FluentValidation
dotnet add src/Application package FluentValidation.DependencyInjectionExtensions
dotnet add src/Application package AutoMapper
dotnet add src/Application package Microsoft.Extensions.Logging.Abstractions

# Infrastructure
dotnet add src/Infrastructure package Microsoft.EntityFrameworkCore
dotnet add src/Infrastructure package Npgsql.EntityFrameworkCore.PostgreSQL
dotnet add src/Infrastructure package Microsoft.EntityFrameworkCore.Tools
dotnet add src/Infrastructure package Microsoft.EntityFrameworkCore.Design
dotnet add src/Infrastructure package Microsoft.AspNetCore.Http.Abstractions
dotnet add src/Infrastructure package MediatR
dotnet add src/Infrastructure package AWSSDK.S3
dotnet add src/Infrastructure package AWSSDK.SimpleEmail
dotnet add src/Infrastructure package AWSSDK.SecretsManager
dotnet add src/Infrastructure package AWSSDK.Extensions.NETCore.Setup

# WebApi
dotnet add src/WebApi package Serilog.AspNetCore
dotnet add src/WebApi package Serilog.Sinks.Console
dotnet add src/WebApi package Serilog.Sinks.Seq
dotnet add src/WebApi package Swashbuckle.AspNetCore
dotnet add src/WebApi package Microsoft.AspNetCore.Authentication.JwtBearer
dotnet add src/WebApi package Asp.Versioning.Mvc
dotnet add src/WebApi package Asp.Versioning.Mvc.ApiExplorer
dotnet add src/WebApi package AspNetCoreRateLimit
dotnet add src/WebApi package Microsoft.Extensions.Diagnostics.HealthChecks.EntityFrameworkCore

# Test packages - Domain.UnitTests
dotnet add tests/Domain.UnitTests package FluentAssertions
dotnet add tests/Domain.UnitTests package Moq
dotnet add tests/Domain.UnitTests package AutoFixture
dotnet add tests/Domain.UnitTests package AutoFixture.Xunit2
dotnet add tests/Domain.UnitTests package Bogus

# Test packages - Application.UnitTests
dotnet add tests/Application.UnitTests package FluentAssertions
dotnet add tests/Application.UnitTests package Moq
dotnet add tests/Application.UnitTests package AutoFixture
dotnet add tests/Application.UnitTests package AutoFixture.Xunit2
dotnet add tests/Application.UnitTests package Bogus
dotnet add tests/Application.UnitTests package MediatR
dotnet add tests/Application.UnitTests package AutoMapper

# Test packages - Infrastructure.IntegrationTests
dotnet add tests/Infrastructure.IntegrationTests package FluentAssertions
dotnet add tests/Infrastructure.IntegrationTests package Testcontainers.PostgreSql
dotnet add tests/Infrastructure.IntegrationTests package Respawn
dotnet add tests/Infrastructure.IntegrationTests package Microsoft.EntityFrameworkCore.InMemory
dotnet add tests/Infrastructure.IntegrationTests package Bogus

# Test packages - Api.IntegrationTests
dotnet add tests/Api.IntegrationTests package FluentAssertions
dotnet add tests/Api.IntegrationTests package Microsoft.AspNetCore.Mvc.Testing
dotnet add tests/Api.IntegrationTests package Testcontainers.PostgreSql
dotnet add tests/Api.IntegrationTests package Respawn
dotnet add tests/Api.IntegrationTests package Bogus
```

## Phase 4: Directory.Build.props

Create `Directory.Build.props` in the solution root with this content:

```xml
<Project>
  <PropertyGroup>
    <TargetFramework>net10.0</TargetFramework>
    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>
    <TreatWarningsAsErrors>false</TreatWarningsAsErrors>
    <AnalysisLevel>latest-recommended</AnalysisLevel>
  </PropertyGroup>
</Project>
```

Then remove `<TargetFramework>`, `<Nullable>`, and `<ImplicitUsings>` from each individual .csproj file since they are now centralized.

## Phase 5: Clean Up

Delete all auto-generated `Class1.cs` files from classlib projects and `UnitTest1.cs` from test projects.

```bash
rm -f src/Domain/Class1.cs
rm -f src/Application/Class1.cs
rm -f src/Infrastructure/Class1.cs
rm -f tests/Domain.UnitTests/UnitTest1.cs
rm -f tests/Application.UnitTests/UnitTest1.cs
rm -f tests/Infrastructure.IntegrationTests/UnitTest1.cs
rm -f tests/Api.IntegrationTests/UnitTest1.cs
```

## Phase 6: .editorconfig

Create `.editorconfig` in solution root:

```ini
root = true

[*]
indent_style = space
indent_size = 4
charset = utf-8
trim_trailing_whitespace = true
insert_final_newline = true

[*.cs]
csharp_style_namespace_declarations = file_scoped:warning
csharp_style_var_for_built_in_types = false:suggestion
csharp_style_var_when_type_is_apparent = true:suggestion
dotnet_sort_system_directives_first = true
```

## Phase 7: .gitignore

Create standard .NET `.gitignore`:

```
bin/
obj/
.vs/
*.user
*.suo
*.DotSettings.user
appsettings.*.local.json
```

## Phase 8: Verify

```bash
dotnet build
```

Build must succeed with zero errors. Fix anything that fails.

Verify Domain.csproj has NO `<PackageReference>` items:
```bash
cat src/Domain/TaskManagement.Domain.csproj
```

If any `<PackageReference>` exists in Domain.csproj, remove it.
