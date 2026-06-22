using tasks.Data;
using tasks.Entities;
using tasks.Exceptions;
using tasks.DTOs;
using tasks.Mappers;

namespace tasks.Services;

public class BookService : IBookService
{
    public Task<IEnumerable<BookResponseDTO>> GetAllAsync(string? author, int page, int pageSize)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 10;

        var query = InMemoryStore.Books.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(author))
        {
            query = query.Where(b => b.Author != null && b.Author.Name.Contains(author, StringComparison.OrdinalIgnoreCase));
        }

        var result = query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(b => b.ToResponse());

        return Task.FromResult<IEnumerable<BookResponseDTO>>(result.ToList());
    }

    public Task<BookResponseDTO> GetByIdAsync(int id)
    {
        var book = InMemoryStore.Books.FirstOrDefault(b => b.Id == id);
        if (book == null)
        {
            throw new BookNotFoundException(id);
        }
        return Task.FromResult(book.ToResponse());
    }

    public Task<BookResponseDTO> CreateAsync(BookCreateDTO dto)
    {
        if (dto == null)
            throw new ArgumentNullException(nameof(dto));

        if (InMemoryStore.Books.Any(b => b.Title.Equals(dto.Title, StringComparison.OrdinalIgnoreCase)))
            throw new InvalidOperationException("A book with the same Title already exists.");

        var author = InMemoryStore.Authors.FirstOrDefault(a => a.Id == dto.AuthorId);
        if (author == null)
            throw new ArgumentException($"Author with ID {dto.AuthorId} does not exist.");

        var book = dto.ToEntity();
        book.Id = InMemoryStore.Books.Any() ? InMemoryStore.Books.Max(b => b.Id) + 1 : 1;
        book.Author = author;

        InMemoryStore.Books.Add(book);
        author.Books.Add(book);

        return Task.FromResult(book.ToResponse());
    }

    public Task<BookResponseDTO> UpdateAsync(int id, BookUpdateDTO dto)
    {
        if (dto == null)
            throw new ArgumentNullException(nameof(dto));

        var existingBook = InMemoryStore.Books.FirstOrDefault(b => b.Id == id);
        if (existingBook == null)
            throw new BookNotFoundException(id);

        if (InMemoryStore.Books.Any(b => b.Id != id && b.Title.Equals(dto.Title, StringComparison.OrdinalIgnoreCase)))
            throw new InvalidOperationException("A book with the same Title already exists.");

        if (existingBook.AuthorId != dto.AuthorId)
        {
            var newAuthor = InMemoryStore.Authors.FirstOrDefault(a => a.Id == dto.AuthorId);
            if (newAuthor == null)
                throw new ArgumentException($"Author with ID {dto.AuthorId} does not exist.");

            existingBook.Author?.Books.Remove(existingBook);
            existingBook.AuthorId = dto.AuthorId;
            existingBook.Author = newAuthor;
            newAuthor.Books.Add(existingBook);
        }

        existingBook.ApplyUpdate(dto);

        return Task.FromResult(existingBook.ToResponse());
    }

    public Task<bool> DeleteAsync(int id)
    {
        var book = InMemoryStore.Books.FirstOrDefault(b => b.Id == id);
        if (book == null)
            return Task.FromResult(false);

        book.Author?.Books.Remove(book);
        InMemoryStore.BookTags.RemoveAll(bt => bt.BookId == id);
        var deleted = InMemoryStore.Books.Remove(book);
        return Task.FromResult(deleted);
    }
}
