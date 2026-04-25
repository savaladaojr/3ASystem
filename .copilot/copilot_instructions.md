# copilot_instructions.md

## Solution Overview

3ASystem is a Blazor-based web application targeting .NET 10. The solution is organized into the following projects:

- **3ASystem.WebUI.Server**: Presentation layer using Blazor and MudBlazor for UI, with components for managing applications, modules, functionalities, and users.
- **3ASystem.Application**: Application layer containing use cases, application services, CQRS handlers, and abstractions for repositories and services.
- **3ASystem.Domain**: Domain layer with core business logic, domain entities, value objects, and domain events.
- **3ASystem.Infrastructure**: Infrastructure layer for data persistence (Entity Framework Core), repository implementations, and external services (e.g., email).
- **3ASystem.Tests.Application / 3ASystem.Tests.Domain**: Automated unit tests using xUnit, FluentAssertions, and NSubstitute.

## Folder Structure

- `src/3ASystem.WebUI.Server/Components/Pages`: Blazor pages for Applications, Modules, Functionalities, Users.
- `src/3ASystem.Application/UseCases`: Application use cases (Commands, Queries, Handlers, Validators, Responses).
- `src/3ASystem.Domain/Entities`: Domain entities for Applications, Modules, Functionalities, Roles, Operations.
- `src/3ASystem.Infrastructure/Data`: EF Core DbContext, repository implementations, entity configurations, migrations.
- `tests/`: Unit tests for Application and Domain layers.

## Main Functionalities

- Application management (CRUD, configuration, modules, functionalities)
- User and role management (CRUD, privileges, authentication, authorization)
- Security and access control (role-based, privilege assignment)
- Modular and extensible architecture for new features

## Key Packages Used

- **Blazor** & **MudBlazor**: UI components and interactive web interface
- **Entity Framework Core**: ORM and data access
- **MediatR**: Mediator pattern for CQRS
- **FluentValidation**: Validation of commands and models
- **Serilog**: Logging
- **xUnit**, **FluentAssertions**, **NSubstitute**: Unit testing and mocking

## Architectural Patterns

- **Clean Architecture**: Separation into Presentation, Application, Domain, and Infrastructure layers
- **CQRS**: Commands and Queries separated for scalability and clarity
- **Repository Pattern**: Abstraction of data access
- **Mediator Pattern**: Decoupling request handling (MediatR)
- **Dependency Injection**: For all services and repositories
- **Domain-Driven Design (DDD)**: Rich domain model, domain events, value objects

## Recommendations for .NET, C#, and Blazor (for this architecture)

- **.NET**: Use latest LTS version, leverage async/await for I/O, prefer dependency injection for all services, keep business logic out of controllers/components.
- **C#**: Follow SOLID principles, use records for immutable value objects, prefer expression-bodied members for simple logic, use pattern matching and nullability annotations.
- **Blazor**: Use component-based design, keep UI logic in code-behind (`.razor.cs`), use MudBlazor for consistent UI, prefer cascading parameters for shared state, avoid business logic in UI components.
- **Testing**: Write unit tests for all application and domain logic, use mocks for infrastructure dependencies, ensure high code coverage for critical paths.
- **Validation**: Use FluentValidation for all input and business rule validation.
- **Logging**: Use Serilog for structured logging, log at appropriate levels (Information, Warning, Error).
- **Security**: Always validate user input, use ASP.NET Core Identity for authentication/authorization, follow least privilege principle for roles/permissions.

---

This file should be updated as the architecture or main practices evolve.
