# BookManager Workspace

This repository contains the solutions for the Day 1 and Day 2 tasks, structured into separate projects.

---

## Workspace Structure

- **[session-1/](./session-1)**: Day 1 tasks. Contains a Console Application demonstrating entity modeling, relational graph seeding, LINQ queries, and a synchronous service layer.
- **[session-2/](./session-2)**: Day 2 tasks. Contains a Web API Application migrating the service layer to asynchronous execution, registering Swagger documentation, and adding custom logging middleware and domain exception handling.

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
