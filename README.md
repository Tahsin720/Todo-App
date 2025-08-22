# TodoApp

A robust, modern Todo application built with ASP.NET Core (.NET 8), Entity Framework Core, and a clean architecture approach. The app supports user authentication, JWT-based authorization, and CRUD operations for todos, each associated with a user.

## Features

- User registration and login with JWT authentication
- Secure password hashing and validation
- CRUD operations for todos, scoped to the authenticated user
- Pagination and filtering support for todo lists
- Global exception handling middleware
- FluentValidation for input validation
- AutoMapper for DTO/entity mapping
- Clean separation of concerns via services, repositories, and controllers

## Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- SQL Server (or compatible, as per your connection string)
- Visual Studio 2022 or later (recommended)

### Configuration

1. **Clone the repository:**


2. **Configure the database:**
- Update the `todoConn` connection string in `appsettings.json` if needed:
  ```json
  "todoConn": "Server=YOUR_SERVER;Database=TODO;Integrated Security=SSPI;TrustServerCertificate=True;"
  ```

3. **Configure JWT settings:**
- The `Tokens` section in `appsettings.json` contains JWT settings. Adjust as needed for production.

### Database Migration

Run the following commands to create and update the database:

> Ensure you have the [EF Core CLI tools](https://docs.microsoft.com/en-us/ef/core/cli/dotnet) installed.

### Running the Application

The API will be available at `https://localhost:5001` or as configured.

---

## API Endpoints

### Auth

- `POST /api/auth/login`  
  Login with email and password.

- `POST /api/auth/signup`  
  Register a new user.

### Todos

- `GET /api/todos`  
  Get paginated todos for the authenticated user.

- `GET /api/todos/{id}`  
  Get a specific todo by ID.

- `POST /api/todos`  
  Create a new todo.

- `PUT /api/todos/{id}`  
  Update an existing todo.

- `DELETE /api/todos/{id}`  
  Delete a todo.

> All `/api/todos` endpoints require authentication.

---

## Project Structure

- `Controllers/` - API controllers
- `Domain/Entities/` - Entity models
- `DataAccess/` - Repositories and query specifications
- `Services/` - Business logic and interfaces
- `Models/` - DTOs and common models
- `Configurations/` - Validation and AutoMapper profiles
- `Middlewares/` - Custom exception handling
- `Utiities/` - Utility classes and custom exceptions
- `DependencyServices/` - Dependency injection setup

---

## Validation

- Uses [FluentValidation](https://fluentvalidation.net/) for DTO validation.
- Validation errors are handled globally via middleware.

---

## Authentication

- Uses [ASP.NET Core Identity](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/identity) for user management.
- JWT tokens are issued on login and required for protected endpoints.

---

## Error Handling

- All exceptions are handled by `CustomExceptionMiddleware` for consistent API error responses.

---

## Design Patterns Used

This project leverages several well-known software design patterns to ensure maintainability, scalability, and testability:

### 1. Repository Pattern
- **Purpose:** Abstracts data access logic and centralizes it in repository classes.
- **Implementation:**  
  - `IGenericRepository<T>` and `GenericRepository<T>` provide generic CRUD operations.
  - `ITodoRepository` and `TodoRepository` offer todo-specific data access.
- **Benefit:** Decouples business logic from data access, making the codebase easier to test and maintain.

### 2. Service Pattern
- **Purpose:** Encapsulates business logic and coordinates between controllers and repositories.
- **Implementation:**  
  - `IGenericService<T>` and `GenericService<T>` for generic business operations.
  - `ITodoService` and `TodoService` for todo-specific business logic.
- **Benefit:** Keeps controllers thin and focused on HTTP concerns, while business rules reside in services.

### 3. Dependency Injection
- **Purpose:** Promotes loose coupling by injecting dependencies rather than hard-coding them.
- **Implementation:**  
  - Service and repository registrations in `DependencyInjection.cs`, `RepositoryInjection.cs`, and `ServiceInjection.cs`.
  - Constructor injection throughout controllers and services.
- **Benefit:** Improves testability and flexibility by allowing easy swapping of implementations.

### 4. Specification Pattern
- **Purpose:** Encapsulates query logic and filtering criteria in reusable specification objects.
- **Implementation:**  
  - `BaseSpecification` and `TodoSpec` define filtering and pagination logic for queries.
- **Benefit:** Promotes reusable, composable, and testable query logic.

### 5. DTO (Data Transfer Object) Pattern
- **Purpose:** Transfers data between layers while hiding internal domain models.
- **Implementation:**  
  - DTOs like `TodoCreateDto`, `TodoUpdateDto`, `LoginDto`, and `UserCreateDto`.
  - AutoMapper profiles for mapping between entities and DTOs.
- **Benefit:** Improves security and decouples API contracts from internal models.

### 6. Middleware Pattern
- **Purpose:** Handles cross-cutting concerns in the HTTP request pipeline.
- **Implementation:**  
  - `CustomExceptionMiddleware` for global exception handling.
- **Benefit:** Centralizes error handling and response formatting.

---

## Author & Email

**Tahsin Habib Brinto**
**tasanhabibbrinto@gmail.com**

---

## License

[MIT](LICENSE)

