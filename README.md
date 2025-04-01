# 3ASystem

![Languages](https://img.shields.io/github/languages/count/savaladaojr/3ASystem)
![Top Language](https://img.shields.io/github/languages/top/savaladaojr/3ASystem)
![C#](https://img.shields.io/badge/language-C%23-blue)
![HTML](https://img.shields.io/badge/language-HTML-blue)
![CSS](https://img.shields.io/badge/language-CSS-blue)
![Blazor](https://img.shields.io/badge/framework-Blazor-blue)
![.NET Core](https://img.shields.io/badge/framework-.NET%20Core-blue)
![EF Core](https://img.shields.io/badge/framework-EF%20Core-blue)
![Total Issues](https://img.shields.io/github/issues/savaladaojr/3ASystem)
![Open Issues](https://img.shields.io/github/issues-raw/savaladaojr/3ASystem)
![License](https://img.shields.io/github/license/savaladaojr/3ASystem)
![Last Commit](https://img.shields.io/github/last-commit/savaladaojr/3ASystem)
![Stars](https://img.shields.io/github/stars/savaladaojr/3ASystem)

## Overview
3ASystem is a Blazor-based web application targeting .NET 9. 
This project aims to provide a robust and scalable system using modern web development practices.
<br><br>

## About It
3A System is a web application developed to manage applications, its modules, functionalities,  and its functionalities, user groups (roles), its users and users' respective privileges.

The 3A System is a comprehensive web application with several key features designed to manage various aspects of applications and user interactions. Here are the main features:

1. **Application Management**:
- *Modules, Functionalities*: Organize and manage different modules and functionalities within each application.
- *Configuration*: Customize settings and preferences for each application.

2. **User Management**:
- *User Roles*: Define and manage user groups (roles) with specific permissions for deferent applications.
- *User Accounts*: Create, update, and delete user accounts for each application.
- *Privileges: Assign and manage user privileges to control access to different parts of the application.

3. **Security and Access Control**:
- *Authentication*: Secure login mechanisms to verify user identities.
- *Authorization*: Ensure users have appropriate access based on their roles and privileges to access the application and its functionalities.

These features make the 3A System a robust solution for managing applications, user roles, and privileges, ensuring both flexibility and security.
<br><br>

## Architecture
The architecture of the 3ASystem project follows the Clean Architecture principles:
- **Presentation Layer**: Implemented using Blazor components for a rich and interactive user interface.
- **Application Layer**: Contains the application logic, including use cases and application services.
- **Domain Layer**: Contains the core business logic and domain entities.
- **Infrastructure Layer**: Manages data persistence, external services, and other infrastructure concerns using Entity Framework Core.
<br><br>

## Design Patterns
The project utilizes several design patterns to ensure maintainability and scalability:
- **Dependency Injection**: To manage dependencies and promote loose coupling between components.
- **Repository Pattern**: To abstract data access logic and provide a clean separation between the business logic and data access layers.
- **Mediator Pattern**: To handle complex workflows and interactions between different parts of the system.
- **CQRS (Command Query Responsibility Segregation)**: To separate read and write operations, improving performance, scalability, and security.
<br><br>

## Principles
The project adheres to the following principles:
- **Clean Code**: Writing code that is easy to understand, maintain, and extend.
- **SOLID Principles**: Ensuring the codebase is modular, flexible, and easy to maintain.
<br><br>

## Used Libraries
The following libraries and frameworks are used in the 3ASystem project:
- **Blazor**: For building interactive web UIs using C#.
- **Entity Framework Core**: For data access and ORM capabilities.
- **MediatR**: For implementing the mediator pattern.
- **FluentValidation**: For validating business rules and data models.
- **AutoMapper**: For object-to-object mapping.
- **Serilog**: For logging and diagnostics.
- **FluentAssertions**: For writing unit tests with clear and readable assertions.
- **xUnit**: For writing unit tests.
- **NSubstitute**: For mocking dependencies in unit tests.
<br><br>

## Automated Unit Tests
The project includes automated unit tests to ensure the reliability and correctness of the codebase.
These tests are written using xUnit and FluentAssertions for clear and readable assertions. 

The main reasons to perform automated unit tests are:
- **Early Bug Detection**: Identify and fix bugs early in the development process.
- **Code Quality**: Ensure the code meets the required quality standards.
- **Refactoring Safety**: Safely refactor code without introducing new bugs.
- **Documentation**: Provide living documentation of the code's expected behavior.
- **Continuous Integration**: Enable continuous integration and delivery by automating the testing process.
<br><br>

## Getting Started
To get started with the 3ASystem project, follow these steps:
1. Clone the repository.
2. Open the solution in Visual Studio.
3. Restore the NuGet packages.
4. Build the solution.
5. Run the application.
<br><br>

## Contributing
Contributions are welcome! Please fork the repository and submit a pull request with your changes.

1. Fork it (https://github.com/savaladaojr/3ASystem/fork);
2. Create your feature branch (e.g. git checkout -b feature/fooBar);
3. Commit your changes (e.g. git commit -am 'Add some fooBar');
4. Push to the branch (e.g. git push origin feature/fooBar);
5. Create a new Pull Request.
<br><br>

## License
This project is licensed under the MIT License.
