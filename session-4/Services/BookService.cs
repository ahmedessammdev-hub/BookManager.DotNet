using Microsoft.EntityFrameworkCore;
using tasks.Data;
using tasks.Entities;
using tasks.Exceptions;
using tasks.DTOs;
using tasks.Mappers;

namespace tasks.Services;

public class BookService : IBookService
{
    private readonly AppDbContext _dbContext;

    public BookService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<BookResponseDTO>> GetAllAsync(string? author, int page, int pageSize)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 10;

        var query = _dbContext.Books
            .AsNoTracking()
            .Include(b => b.Author)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(author))
        {
            query = query.Where(b => b.Author.Name.Contains(author));
        }

        var result = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(b => b.ToResponse())
            .ToListAsync();

        return result;
    }

    public async Task<BookResponseDTO> GetByIdAsync(int id)
    {
        var book = await _dbContext.Books
            .AsNoTracking()
            .Include(b => b.Author)
            .FirstOrDefaultAsync(b => b.Id == id);

        if (book == null)
        {
            throw new BookNotFoundException(id);
        }

        return book.ToResponse();
    }

    public async Task<BookResponseDTO> CreateAsync(BookCreateDTO dto)
    {
        if (dto == null)
            throw new ArgumentNullException(nameof(dto));

        if (await _dbContext.Books.AnyAsync(b => b.Title == dto.Title))
            throw new InvalidOperationException("A book with the same Title already exists.");

        var author = await _dbContext.Authors.FindAsync(dto.AuthorId);
        if (author == null)
            throw new ArgumentException($"Author with ID {dto.AuthorId} does not exist.");

        var book = dto.ToEntity();
        book.Author = author;

        _dbContext.Books.Add(book);
        await _dbContext.SaveChangesAsync();

        return book.ToResponse();
    }

    public async Task<BookResponseDTO> UpdateAsync(int id, BookUpdateDTO dto)
    {
        if (dto == null)
            throw new ArgumentNullException(nameof(dto));

        var existingBook = await _dbContext.Books
            .Include(b => b.Author)
            .FirstOrDefaultAsync(b => b.Id == id);

        if (existingBook == null)
            throw new BookNotFoundException(id);

        if (await _dbContext.Books.AnyAsync(b => b.Id != id && b.Title == dto.Title))
            throw new InvalidOperationException("A book with the same Title already exists.");

        if (existingBook.AuthorId != dto.AuthorId)
        {
            var newAuthor = await _dbContext.Authors.FindAsync(dto.AuthorId);
            if (newAuthor == null)
                throw new ArgumentException($"Author with ID {dto.AuthorId} does not exist.");

            existingBook.AuthorId = dto.AuthorId;
            existingBook.Author = newAuthor;
        }

        existingBook.ApplyUpdate(dto);
        await _dbContext.SaveChangesAsync();

        return existingBook.ToResponse();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var book = await _dbContext.Books.FindAsync(id);
        if (book == null)
            return false;

        var bookTags = _dbContext.BookTags.Where(bt => bt.BookId == id);
        _dbContext.BookTags.RemoveRange(bookTags);

        _dbContext.Books.Remove(book);
        var affected = await _dbContext.SaveChangesAsync();
        return affected > 0;
    }
}
