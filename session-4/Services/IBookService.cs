using tasks.DTOs;

namespace tasks.Services;

public interface IBookService
{
    Task<IEnumerable<BookResponseDTO>> GetAllAsync(string? author, int page, int pageSize);
    Task<BookResponseDTO> GetByIdAsync(int id);
    Task<BookResponseDTO> CreateAsync(BookCreateDTO dto);
    Task<BookResponseDTO> UpdateAsync(int id, BookUpdateDTO dto);
    Task<bool> DeleteAsync(int id);
}
