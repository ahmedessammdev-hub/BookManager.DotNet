using tasks.Entities;

namespace tasks.Services;

public interface IBookService
{
    IEnumerable<Book> GetAll();
    Book? GetById(int id);
    void Create(Book book);
    void Update(Book book);
    bool Delete(int id);
}
