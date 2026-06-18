# .NET Day 1 Tasks - Book Management Console Application

A clean, modern, and professional C# Console Application demonstrating C# entity modeling, in-memory data seeding, LINQ query operations, and the Service/Repository design pattern. Built using modern .NET practices with C# file-scoped namespaces, nullable reference types, and expression-bodied members.

---

## 📂 Project Structure

The codebase is organized into structured directories to enforce separation of concerns, simulating a production-grade application architecture:

```text
tasks/
│
├── tasks/
│   ├── Entities/             # Domain Model Entities
│   │   ├── Author.cs         # Author model with navigation properties
│   │   ├── Book.cs           # Book model with reference keys & properties
│   │   ├── Tag.cs            # Tag model for categorization
│   │   └── BookTag.cs        # Join-table entity for Book-Tag relations
│   │
│   ├── Data/                 # Data Layer
│   │   └── InMemoryStore.cs  # Static store representing database lists & seed data
│   │
│   ├── Services/             # Business Logic Layer
│   │   ├── IBookService.cs   # Interface contract for Book CRUD operations
│   │   └── BookService.cs    # Implementation of Book CRUD against InMemoryStore
│   │
│   ├── Program.cs            # Application entry point & demonstration runner
│   └── tasks.csproj          # .NET Project Configuration file
│
└── README.md                 # Project Documentation
```

---

## 🛠️ Task Implementations

### 1️⃣ Domain Models (Task 1.1)
The application defines four core entity classes to represent relational database entities:
*   **[Author.cs](file:///d:/work/Day1%20tasks%20.net/tasks/tasks/Entities/Author.cs)**: Represents a book writer. Contains an `Id`, a `Name` (English), and a navigation collection property `ICollection<Book> Books`.
*   **[Book.cs](file:///d:/work/Day1%20tasks%20.net/tasks/tasks/Entities/Book.cs)**: Represents a published literary work. Contains `Id`, `Title` (English), `Year` (publication year), `PageCount`, `AuthorId` (foreign key), and an `Author` navigation property.
*   **[Tag.cs](file:///d:/work/Day1%20tasks%20.net/tasks/tasks/Entities/Tag.cs)**: Represents book categorizations. Contains `Id` and `Name` (English, e.g. *Horror*, *Novel*).
*   **[BookTag.cs](file:///d:/work/Day1%20tasks%20.net/tasks/tasks/Entities/BookTag.cs)**: Represents the many-to-many join relationship between books and tags. Contains `BookId` and `TagId`.

### 2️⃣ In-Memory Store & Seeding (Task 1.2)
The **[InMemoryStore.cs](file:///d:/work/Day1%20tasks%20.net/tasks/tasks/Data/InMemoryStore.cs)** class holds static lists acting as databases.
During startup (static constructor), the store seeds:
*   **3 Authors** (Ahmed Khaled Tawfik, Naguib Mahfouz, Mustafa Mahmoud).
*   **8 Books** (English titles, including details like title, year, page count).
*   **4 Tags** (*Horror*, *Novel*, *Philosophy*, *Fiction*).
*   **6 Book-Tag mappings**.

It also runs a **relational fix-up loop** mapping instances of `Book.Author` and adding books to `Author.Books` so the object graph navigation mimics an ORM (like Entity Framework Core) in-memory behavior.

### 3️⃣ Advanced LINQ Queries (Task 1.3)
All queries are implemented in `Program.cs` utilizing deferred execution and the C# LINQ API:
1.  **Filter by Author**: Retrieves books published by a specific author based on their identifier.
2.  **Select & Sort**: Pulls only book titles, ordered alphabetically (`OrderBy`).
3.  **Group By**: Groups books by their Author, projecting into an anonymous type containing the author's name and the total number of books.
4.  **Aggregate (Average)**: Calculates the average number of pages across all books (`Average`).
5.  **Existence Check (Any)**: Uses `Any` to check if there is at least one book exceeding 500 pages.
6.  **Find by ID**: Queries the collection using `FirstOrDefault` to locate a specific book.
7.  **Relational Join**: Explicitly joins the `Books` collection with the `Authors` collection on their matching keys.
8.  **Subset / Selection**: Orders books descending by page count and pulls the top 3 (`OrderByDescending` + `Take`).

### 4️⃣ Service Layer - CRUD Operations (Tasks 1.4 & 1.5)
The **[IBookService](file:///d:/work/Day1%20tasks%20.net/tasks/tasks/Services/IBookService.cs)** interface declares operations standard for business layers:
```csharp
public interface IBookService
{
    IEnumerable<Book> GetAll();
    Book? GetById(int id);
    void Create(Book book);
    void Update(Book book);
    bool Delete(int id);
}
```

The concrete implementation **[BookService](file:///d:/work/Day1%20tasks%20.net/tasks/tasks/Services/BookService.cs)** operates against `InMemoryStore` and incorporates:
*   **Input Validations**: Throws `ArgumentNullException` on null entries.
*   **Constraint Checking**: Prevents duplication of IDs or titles, throws `InvalidOperationException`.
*   **Relationship Synchronization**: When a book is created, updated, or deleted, the service maintains list integrity. For instance, deleting a book removes it from the static store, pulls it from the author's list of written works, and deletes all linked tag connections.

---

## 🚀 How to Run the Application

### Prerequisites
*   [.NET SDK 8.0 or 10.0](https://dotnet.microsoft.com/download) installed on your system.

### Build and Run
Open your terminal inside the project directory and execute the following commands:

```bash
# Build the project to verify compilation
dotnet build

# Run the console application
dotnet run
```

---

## 💻 Sample Console Output

Upon running the program, the console output details each LINQ query result followed by a live demonstration of `BookService` CRUD operations:

```text
==================================================
          .NET DAY 1 TASKS DEMONSTRATION          
==================================================

--- TASK 1.3: LINQ Queries ---

1. Books by Ahmed Khaled Tawfik (AuthorId = 1):
   - Paranormal (1993), 150 pages
   - Utopia (2008), 192 pages

2. Book titles sorted alphabetically:
   - Dialogue with an Atheist Friend
   - My Journey from Doubt to Belief
   - Palace of Desire
   - Palace Walk
   - Paranormal
   - Sugar Street
   - The Spider
   - Utopia

3. Group by Author:
   - Ahmed Khaled Tawfik: 2 book(s)
   - Naguib Mahfouz: 3 book(s)
   - Mustafa Mahmoud: 3 book(s)

4. Average pages across all books: 314.0 pages

5. Any book has more than 500 pages? True

6. Book with Id = 3: 'Palace Walk' by Naguib Mahfouz

7. Join Books and Authors:
   - 'Paranormal' written by Ahmed Khaled Tawfik
   - 'Utopia' written by Ahmed Khaled Tawfik
   - 'Palace Walk' written by Naguib Mahfouz
   - ...

8. Top 3 longest books:
   - My Journey from Doubt to Belief (520 pages)
   - Palace of Desire (460 pages)
   - Palace Walk (420 pages)

==================================================
--- TASK 1.4 & 1.5: IBookService CRUD Operations ---

Initial total books list: 8
Retrieved Book 1: Paranormal (Author: Ahmed Khaled Tawfik)

Creating a new book...
New Book ID assigned: 9
Book count after creation: 9

Updating the newly created book...
Updated Book Title: Paranormal - The Legend of Al-Naddaha (Special Edition) - PageCount: 135

Deleting book with ID 9...
Delete successful? True
Book count after deletion: 8
==================================================
```
