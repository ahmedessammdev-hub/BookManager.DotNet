# BookManager Workspace

This repository contains the solutions for all .NET Sessions, structured into separate project folders.

---

## Workspace Structure

- **[session-1/](./session-1)**: Day 1 tasks. A Console Application demonstrating entity modeling, relational graph seeding, LINQ queries, and a synchronous service layer.
- **[session-2/](./session-2)**: Day 2 tasks. A Web API Application migrating the service layer to asynchronous execution, registering Swagger documentation, and adding custom logging middleware and domain exception handling.
- **[session-3/](./session-3)**: Day 3 tasks. A DTO-driven Web API implementing validation decorators, static object mapping, search filters, pagination, and clean controller separations.
- **[session-4/](./session-4)**: Day 4 tasks. A persistent Web API using Entity Framework Core with MySQL, database migrations, dynamic security conventions, and JWT Authentication.

---

## Projects Overview

### Session 1: Console Application (session-1)
A command-line implementation showcasing in-memory data operations and LINQ queries using synchronous execution.

#### Key Features
- Domain Modeling: Author, Book, Tag, and BookTag entities.
- Relational Seeding: Relation fix-up loop mapping object references in the in-memory data store.
- LINQ Operations: Projections, grouping, ordering, joins, and aggregates.
- Service Layer: BookService implementation handling input validation and relationship synchronization.

#### How to Run
Run the console application from the workspace root:
```bash
dotnet run --project session-1
```

---

### Session 2: Web API Application (session-2)
An ASP.NET Core Web API migration utilizing async patterns and custom middleware.

#### Key Features
- Web App Migration: Project configured to use Microsoft.NET.Sdk.Web.
- Asynchronous Refactoring: Updated service layer to use Task and Task<T> return types.
- REST Controller: Added BooksController exposing GET /api/books.
- Custom Middleware: RequestLoggingMiddleware utilizing a Stopwatch to log HTTP method, path, status, and response time.
- Custom Exceptions: BookNotFoundException thrown on missing resource retrievals.

#### How to Run
Run the API application from the workspace root:
```bash
dotnet run --project session-2
```
Once the server starts, navigate to `http://localhost:5005` in your browser to view the interactive Swagger UI.

---

### Session 3: DTO-Driven Web API (session-3)
An ASP.NET Core Web API refactoring that introduces Data Transfer Objects, model validation, and query filtering.

#### Key Features
- DTOs & Validation: Created request/response DTOs decorated with validation attributes (e.g., Required, Range).
- Manual Object Mapping: Static extension methods mapping between Entities and DTOs, avoiding reflection overhead.
- Service Refactoring: Updated BookService with asynchronous Author filtering and pagination (`Skip`/`Take`).
- Clean separation: Kept BooksController separated from domain entities using response records.

#### How to Run
Run the API application from the workspace root:
```bash
dotnet run --project session-3
```

---

### Session 4: Relational Database & JWT Secured Web API (session-4)
An ASP.NET Core Web API integrated with Entity Framework Core (MySQL) and secured with JWT Bearer authentication.

#### Key Features
- EF Core & MySQL: Replaced in-memory data structures with a persistent database via `AppDbContext`.
- Database Migrations & Seeding: Configured Fluent API seed data and managed schema changes (`InitialCreate` & `AddIsbnToBook`).
- Async EF Queries: Updated service layer using database eager loading (`Include`) and tracking optimizations (`AsNoTracking`).
- JWT Authentication: Added endpoints `/auth/login` and `/auth/me` to distribute and decode user tokens.
- Dynamic Security Conventions: Automatically applied authentication/authorization rules to the unmodified `BooksController` via a dynamic startup convention.

#### How to Run
Apply migrations and run the API from the workspace root:
```bash
# Apply migrations to local MySQL instance
dotnet ef database update --project session-4

# Start the Web API server
dotnet run --project session-4
```
Once started, the API is accessible at `http://localhost:5005` with interactive Swagger UI support.
