# .NET Session 4 Tasks - Book Management Web API with MySQL & JWT

This project builds upon the Session 3 DTO-driven API by replacing the in-memory store with a persistent MySQL/MariaDB database using Entity Framework Core. Additionally, it implements secure API endpoints using JWT (JSON Web Tokens) with role-based access control, while keeping the main `BooksController` source code entirely unchanged.

---

## Project Structure

The project has been upgraded with database support and security layers:

```text
session-4/
│
├── Controllers/          # API Controllers
│   ├── BooksController.cs # UNCHANGED: DTO-driven endpoints secured dynamically
│   └── AuthController.cs  # NEW: Token generation (/auth/login) and user claims (/auth/me)
│   
├── DTOs/                 # Data Transfer Objects
│   ├── BookCreateDTO.cs
│   ├── BookUpdateDTO.cs
│   └── BookResponseDTO.cs
│   
├── Mappers/              # Object Mapping
│   └── BookMapper.cs
│   
├── Exceptions/           # Domain Exceptions
│   └── BookNotFoundException.cs
│   
├── Middleware/           # Request Pipeline Middleware
│   └── RequestLoggingMiddleware.cs
│   
├── Entities/             # Domain Entities (Added Isbn property)
│   ├── Author.cs
│   ├── Book.cs
│   ├── BookTag.cs
│   └── Tag.cs
│   
├── Data/                 # Persistence Layer
│   └── AppDbContext.cs   # NEW: EF Core MySQL DB Context & seed data
│   
├── Services/             # Business Logic Layer
│   ├── IBookService.cs
│   └── BookService.cs    # MODIFIED: Rewritten to query DB via EF Core (AsNoTracking, Include, Skip/Take)
│   
├── Program.cs            # MODIFIED: MySQL wiring, JWT validation, and dynamic Security Conventions
├── appsettings.json      # NEW: DB connection strings & JWT keys
└── Session4.csproj       # MODIFIED: References MySQL provider & JWT bearer libraries
```

---

## Task Implementations

### 1. Database Integration & Seeding (Tasks 4.1 - 4.3)
- Created `AppDbContext` to register entities (`Book`, `Author`, `Tag`, `BookTag`) and map database configurations, including composite primary keys on the `BookTag` relationship.
- Seeded initial mock data (3 authors, 8 books, 4 tags, and 6 tag links) using Fluent API model seeding (`HasData`).
- Registered Pomelo's MySQL database provider in `Program.cs`.
- Scaffolded and ran EF migrations (`InitialCreate`) to create tables in the local database.

### 2. Service Layer Refactoring (Task 4.4 - 4.5)
- Deleted the obsolete `InMemoryStore.cs` file.
- Rewrote `BookService.cs` using the database context asynchronously:
  - Employed `AsNoTracking()` for read-only optimizations.
  - Employed `Include(b => b.Author)` (Eager Loading) to resolve relations.
  - Implemented pagination via database-level `Skip` and `Take`.
- Verified that `BooksController.cs` remains **UNCHANGED** from Session 3.

### 3. ISBN Database Evolution (Task 4.6)
- Added `Isbn` property (`string`) to `Book`.
- Created a new migration `AddIsbnToBook`.
- Tested the database migration lifecycle by successfully applying, reverting, and re-applying it.

### 4. JWT Authentication & Conventions (Tasks 4.7 - 4.8)
- Added a `POST /auth/login` endpoint that accepts hardcoded credentials and returns a JWT:
  - `admin` / `password` (Role: `Admin`)
  - `user` / `password` (Role: `User`)
- Added a `GET /auth/me` endpoint to read and output claims from the authenticated user context.
- Implemented `SecurityRequirementsConvention` dynamically inside `Program.cs` to apply `[Authorize]` (Users & Admins) on GET actions and `[Authorize(Roles = "Admin")]` on POST/PUT/DELETE actions for the `Books` controller without changing the controller's source code.
- Added authorization capabilities directly into Swagger UI to allow token submissions.

---

## How to Run and Test

### 1. Prerequisites
- [.NET SDK 10.0](https://dotnet.microsoft.com/download) installed.
- MySQL or MariaDB server running locally on port `3306`.

### 2. Database Connection
Adjust the database connection parameters in `session-4/appsettings.json` if needed:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Port=3306;Database=bookmanager;User=root;Password=YOUR_PASSWORD;"
}
```

### 3. Run Migrations
From the workspace root directory, apply the schema to your MySQL instance:
```bash
dotnet ef database update --project session-4
```

### 4. Build and Run
Start the Web API server:
```bash
dotnet run --project session-4
```

The server runs on **`http://localhost:5005`**. Open this URL in your browser to view the Swagger UI.

### 5. API Authentication Testing Flow
1. Open the Swagger page at `http://localhost:5005`.
2. Make a request to `GET /api/books` -> Returns `401 Unauthorized`.
3. Invoke `POST /auth/login` with `user` credentials -> Copy the token from the response.
4. Click the **Authorize** button at the top, enter `Bearer [copied_token]`, and authorize.
5. Invoke `GET /api/books` -> Returns `200 OK` with the book list.
6. Try invoking `POST /api/books` -> Returns `403 Forbidden` (role must be Admin).
7. Login again as `admin` -> Copy the new token and authorize.
8. Invoke `POST /api/books` -> Returns `201 Created` successfully.
9. Call `GET /auth/me` -> Displays user identifier and role claims.
