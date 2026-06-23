# .NET Day 3 Tasks - Book Management Web API Application

This project builds upon the Session 2 asynchronous Web API by refactoring the request/response handling to use Data Transfer Objects (DTOs) and a manual mapper. It introduces model validation, query filtering, pagination, and consistent RESTful endpoint responses.

---

## Project Structure

The project has been enhanced with DTO and Mapper layers:

```text
session-3/
│
├── Controllers/          # API Controllers
│   └── BooksController.cs # DTO-driven REST endpoints with ProblemDetails error mapping
│   
├── DTOs/                 # Data Transfer Objects (Request/Response Models)
│   ├── BookCreateDTO.cs  # Validation-decorated payload class for creation
│   ├── BookUpdateDTO.cs  # Validation-decorated payload class for updates
│   └── BookResponseDTO.cs # Immutable record payload representing a Book response
│   
├── Mappers/              # Manual Object Mapper
│   └── BookMapper.cs     # Static extension methods mapping between Entities and DTOs
│   
├── Exceptions/           # Custom Domain Exceptions
│   └── BookNotFoundException.cs
│   
├── Middleware/           # HTTP Request Pipeline Middleware
│   └── RequestLoggingMiddleware.cs
│   
├── Entities/             # Domain Model Entities (from Day 1)
│   ├── Author.cs
│   ├── Book.cs
│   ├── BookTag.cs
│   └── Tag.cs
│   
├── Data/                 # Data Layer (from Day 1)
│   └── InMemoryStore.cs
│   
├── Services/             # Business Logic Layer (Asynchronous & DTO-driven)
│   ├── IBookService.cs   # DTO-oriented async service contract
│   └── BookService.cs    # Async implementation with filtering, pagination, and mapping
│   
├── Program.cs            # API configuration, middleware pipeline, and dependency injection
└── Session3.csproj       # Web SDK configuration file (.NET 10.0)
```

---

## Task Implementations

### DTOs & Validation (Task 3.1)
- Created `BookCreateDTO` and `BookUpdateDTO` classes using `System.ComponentModel.DataAnnotations` to validate request payloads (e.g. required title, positive page counts, valid year range).
- Created `BookResponseDTO` as a C# `record` with `init` properties. This flat structure eliminates serialization loops (circular dependencies) when returning data.

### Manual Object Mapping (Task 3.2)
- Implemented `BookMapper` providing extension methods (`ToEntity`, `ToResponse`, `ApplyUpdate`) to copy data between entities and DTOs manually without using high-overhead reflection mappers (like AutoMapper).

### Interface & Service Refactoring (Tasks 3.3 & 3.4)
- Updated `IBookService` to accept and return DTO models rather than domain entity objects.
- Enhanced `BookService.GetAllAsync` with `author` string filtering (case-insensitive substring contains match) and pagination using `Skip` and `Take` based on `page` and `pageSize` parameters.

### Service Registration (Task 3.5)
- Verified `IBookService` is registered as Scoped in `Program.cs`.
- Replaced any direct instantiations of the service (e.g., `new BookService()`) in the codebase with Constructor Dependency Injection.

### REST Controller Endpoints & Testing (Tasks 3.6 & 3.7)
- Fully implemented standard REST actions in `BooksController.cs`:
  - `GET /api/books`: Supports optional query filters `author`, `page`, and `pageSize`.
  - `GET /api/books/{id}`: Returns `200 OK` or `404 ProblemDetails` if not found.
  - `POST /api/books`: Returns `201 Created` with the location header, or auto `400 Bad Request` if payload validation fails (e.g., empty title).
  - `PUT /api/books/{id}`: Updates the book details and returns `204 No Content`.
  - `DELETE /api/books/{id}`: Removes the book and returns `204 No Content`.
- Ensures **zero** references to the `Book` domain entity class exist in the controller, achieving strict separation of concerns.

---

## How to Run and Test

### Prerequisites
- [.NET SDK 8.0 or 10.0](https://dotnet.microsoft.com/download) installed.

### Build and Run
From the workspace root directory, run:
```bash
# Start the Web API server
dotnet run --project session-3
```

The server runs on **`http://localhost:5005`**. Use the Swagger UI page at the root address to test all endpoints.
