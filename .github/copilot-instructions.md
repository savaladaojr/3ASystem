# Copilot instructions

## Build and tests

Run these commands from the repository root:

```powershell
dotnet build 3ASystem.sln
dotnet test 3ASystem.sln
```

Run one test by filtering on its fully qualified name:

```powershell
dotnet test tests\3ASystem.Tests.Application\3ASystem.Tests.Application.csproj --filter "FullyQualifiedName~CreateApplicationCommandHandlerTests.CreateApplicationCommandHandler_Should_ReturnFailResult_WhenAbbreviationIsNotUnique"
```

Projects target .NET 10. Application and domain tests are separate projects under `tests\`.

## Solution overview

3ASystem is a Blazor web application organized into presentation, application, domain, and infrastructure layers. 
Its main capabilities include application, module, functionality, and operation management, as well as user, role, and privilege management.


- `3ASystem.WebUI.Server` provides the Blazor and MudBlazor interface and ASP.NET Core Identity.
- `3ASystem.Application` contains use cases, services, and abstractions.
- `3ASystem.Domain` contains business rules, entities, value objects, and domain events.
- `3ASystem.Infrastructure` implements persistence with EF Core and SQL Server, repositories, and external services.
- `tests\` contains the application and domain unit-test projects.

## Architecture

The solution follows a layered design. `3ASystem.WebUI.Server` hosts interactive-server Blazor components and ASP.NET Core Identity; its `Program.cs` wires up the application and infrastructure layers. 
The application layer references the domain and holds use cases and abstractions. 
Infrastructure references application, implements its repository and unit-of-work abstractions with EF Core/SQL Server, and registers persistence and services.

Application behavior is organized as MediatR commands and queries. 
FluentValidation validators and logging/validation pipeline behaviors are registered in `3ASystem.Application\DependenciesResolver.cs`. 
Use cases depend on application abstractions rather than EF implementations. 

Domain entities own business state and raise domain events; `ApplicationDbContext` dispatches those events after saving. 
Identity uses a separate `BlazorAppDbContext`.

The codebase uses dependency injection and repository abstractions to keep application use cases independent of infrastructure. 
Keep business logic in the application or domain layers rather than in UI components.


## Repository conventions

- Add use cases under `3ASystem.Application\UseCases\<Area>\Commands` or `Queries`; the command/query folders typically contain a folder per operation, with its handler, validator, and related response types.
- Keep repository, data-context, and unit-of-work contracts in `3ASystem.Application\Abstractions\Data`; implement them in `3ASystem.Infrastructure\Data`.
- Domain entities are grouped by business area under `3ASystem.Domain\Entities`. Follow the existing factory and typed-ID patterns, and represent expected domain/application failures with the shared `Result`/`Error` types.
- Blazor page components are under `3ASystem.WebUI.Server\Components\Pages`. Existing forms and lists pair `.razor` markup with `.razor.cs` code-behind and use MudBlazor.
- Tests mirror the layers and feature areas: handler/validator tests are in `3ASystem.Tests.Application`, and entity tests are in `3ASystem.Tests.Domain`. Existing tests use xUnit, FluentAssertions, and NSubstitute.
- For changes to operations, consult the implementation notes in `docs\Feature_Operations\`; use the current code as the source of truth where older planning documents differ.
- Use asynchronous APIs for I/O, constructor-injected dependencies, and the existing component code-behind pattern. Follow existing C# conventions and nullability patterns rather than introducing a different style.
- Use clear English identifiers, with PascalCase for types and methods and camelCase for local variables. Prefer immutable `record` types for value objects and DTOs where appropriate; apply SOLID and DRY without departing from established patterns.
- Validate inputs with FluentValidation and use ASP.NET Core Identity and the existing authorization model for authentication and access control. Apply least privilege when changing roles or privileges.
- Keep logs structured and at appropriate levels, following the existing logging and pipeline behavior configuration.
- For EF Core queries, avoid N+1 access patterns and project only the data the caller needs.
