using tasks.Data;
using tasks.Entities;

namespace tasks.Services;

public class BookService : IBookService
{
    public IEnumerable<Book> GetAll()
    {
        return InMemoryStore.Books;
    }

    public Book? GetById(int id)
    {
        return InMemoryStore.Books.FirstOrDefault(b => b.Id == id);
    }

    public void Create(Book book)
    {
        if (book == null)
            throw new ArgumentNullException(nameof(book));

        if (InMemoryStore.Books.Any(b => b.Id == book.Id || b.Title.Equals(book.Title, StringComparison.OrdinalIgnoreCase)))
            throw new InvalidOperationException("A book with the same ID or Title already exists.");

        var author = InMemoryStore.Authors.FirstOrDefault(a => a.Id == book.AuthorId);
        if (author == null)
            throw new ArgumentException($"Author with ID {book.AuthorId} does not exist.");

        if (book.Id <= 0)
        {
            book.Id = InMemoryStore.Books.Any() ? InMemoryStore.Books.Max(b => b.Id) + 1 : 1;
        }

        book.Author = author;
        InMemoryStore.Books.Add(book);
        author.Books.Add(book);
    }

    public void Update(Book book)
    {
        if (book == null)
            throw new ArgumentNullException(nameof(book));

        var existingBook = InMemoryStore.Books.FirstOrDefault(b => b.Id == book.Id);
        if (existingBook == null)
            throw new KeyNotFoundException($"Book with ID {book.Id} not found.");

        if (existingBook.AuthorId != book.AuthorId)
        {
            var newAuthor = InMemoryStore.Authors.FirstOrDefault(a => a.Id == book.AuthorId);
            if (newAuthor == null)
                throw new ArgumentException($"Author with ID {book.AuthorId} does not exist.");

            existingBook.Author?.Books.Remove(existingBook);
            existingBook.AuthorId = book.AuthorId;
            existingBook.Author = newAuthor;
            newAuthor.Books.Add(existingBook);
        }

        existingBook.Title = book.Title;
        existingBook.Year = book.Year;
        existingBook.PageCount = book.PageCount;
    }

    public bool Delete(int id)
    {
        var book = InMemoryStore.Books.FirstOrDefault(b => b.Id == id);
        if (book == null)
            return false;

        book.Author?.Books.Remove(book);
        InMemoryStore.BookTags.RemoveAll(bt => bt.BookId == id);
        return InMemoryStore.Books.Remove(book);
    }
}
