using System;
using System.Linq;
using tasks.Data;
using tasks.Entities;
using tasks.Services;

namespace tasks;

class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        Console.WriteLine("==================================================");
        Console.WriteLine("          .NET DAY 1 TASKS DEMONSTRATION          ");
        Console.WriteLine("==================================================");
        Console.WriteLine();

        Console.WriteLine("--- TASK 1.3: LINQ Queries ---");

        // Query 1: Filter by author
        var authorBooks = InMemoryStore.Books.Where(b => b.AuthorId == 1).ToList();
        Console.WriteLine("\n1. Books by Ahmed Khaled Tawfik (AuthorId = 1):");
        foreach (var book in authorBooks)
        {
            Console.WriteLine($"   - {book.Title} ({book.Year}), {book.PageCount} pages");
        }

        // Query 2: Sort alphabetically
        var sortedTitles = InMemoryStore.Books.Select(b => b.Title).OrderBy(t => t).ToList();
        Console.WriteLine("\n2. Book titles sorted alphabetically:");
        foreach (var title in sortedTitles)
        {
            Console.WriteLine($"   - {title}");
        }

        // Query 3: Group by author
        var booksGroupedByAuthor = InMemoryStore.Books
            .GroupBy(b => b.AuthorId)
            .Select(g => new
            {
                AuthorId = g.Key,
                AuthorName = InMemoryStore.Authors.FirstOrDefault(a => a.Id == g.Key)?.Name ?? "Unknown",
                BookCount = g.Count()
            }).ToList();
        Console.WriteLine("\n3. Group by Author:");
        foreach (var group in booksGroupedByAuthor)
        {
            Console.WriteLine($"   - {group.AuthorName}: {group.BookCount} book(s)");
        }

        // Query 4: Average pages
        double averagePageCount = InMemoryStore.Books.Average(b => b.PageCount);
        Console.WriteLine($"\n4. Average pages across all books: {averagePageCount:F1} pages");

        // Query 5: Thick books check
        bool hasThickBook = InMemoryStore.Books.Any(b => b.PageCount > 500);
        Console.WriteLine($"\n5. Any book has more than 500 pages? {hasThickBook}");

        // Query 6: Find by ID
        var bookById = InMemoryStore.Books.FirstOrDefault(b => b.Id == 3);
        Console.WriteLine($"\n6. Book with Id = 3: {(bookById != null ? $"'{bookById.Title}' by {bookById.Author?.Name}" : "Not Found")}");

        // Query 7: Join books and authors
        var bookAuthorJoin = InMemoryStore.Books
            .Join(
                InMemoryStore.Authors,
                book => book.AuthorId,
                author => author.Id,
                (book, author) => new { book.Title, AuthorName = author.Name }
            ).ToList();
        Console.WriteLine("\n7. Join Books and Authors:");
        foreach (var item in bookAuthorJoin)
        {
            Console.WriteLine($"   - '{item.Title}' written by {item.AuthorName}");
        }

        // Query 8: Top 3 longest books
        var top3LongestBooks = InMemoryStore.Books
            .OrderByDescending(b => b.PageCount)
            .Take(3)
            .ToList();
        Console.WriteLine("\n8. Top 3 longest books:");
        foreach (var book in top3LongestBooks)
        {
            Console.WriteLine($"   - {book.Title} ({book.PageCount} pages)");
        }

        Console.WriteLine("\n==================================================");
        Console.WriteLine("--- TASK 1.4 & 1.5: IBookService CRUD Operations ---");

        IBookService bookService = new BookService();

        Console.WriteLine($"\nInitial total books list: {bookService.GetAll().Count()}");

        int testId = 1;
        var bookToFind = bookService.GetById(testId);
        Console.WriteLine($"Retrieved Book {testId}: {bookToFind?.Title ?? "None"} (Author: {bookToFind?.Author?.Name ?? "None"})");

        Console.WriteLine("\nCreating a new book...");
        var newBook = new Book
        {
            Title = "Paranormal - The Legend of Al-Naddaha",
            Year = 1993,
            PageCount = 120,
            AuthorId = 1
        };
        bookService.Create(newBook);
        Console.WriteLine($"New Book ID assigned: {newBook.Id}");
        Console.WriteLine($"Book count after creation: {bookService.GetAll().Count()}");

        Console.WriteLine("\nUpdating the newly created book...");
        newBook.Title = "Paranormal - The Legend of Al-Naddaha (Special Edition)";
        newBook.PageCount = 135;
        bookService.Update(newBook);
        var updatedBook = bookService.GetById(newBook.Id);
        Console.WriteLine($"Updated Book Title: {updatedBook?.Title} - PageCount: {updatedBook?.PageCount}");

        Console.WriteLine($"\nDeleting book with ID {newBook.Id}...");
        bool isDeleted = bookService.Delete(newBook.Id);
        Console.WriteLine($"Delete successful? {isDeleted}");
        Console.WriteLine($"Book count after deletion: {bookService.GetAll().Count()}");
        Console.WriteLine("==================================================");
    }
}
