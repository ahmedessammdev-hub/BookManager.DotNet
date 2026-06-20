namespace tasks.Exceptions;

public class BookNotFoundException : Exception
{
    public BookNotFoundException(int id) 
        : base($"Book with ID {id} was not found.")
    {
    }
}
