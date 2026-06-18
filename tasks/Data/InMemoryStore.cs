using tasks.Entities;

namespace tasks.Data;

public static class InMemoryStore
{
    public static List<Author> Authors { get; } = new();
    public static List<Book> Books { get; } = new();
    public static List<Tag> Tags { get; } = new();
    public static List<BookTag> BookTags { get; } = new();

    static InMemoryStore()
    {
        Seed();
    }

    private static void Seed()
    {
        Authors.AddRange(new List<Author>
        {
            new() { Id = 1, Name = "Ahmed Khaled Tawfik" },
            new() { Id = 2, Name = "Naguib Mahfouz" },
            new() { Id = 3, Name = "Mustafa Mahmoud" }
        });

        Books.AddRange(new List<Book>
        {
            new() { Id = 1, Title = "Paranormal", Year = 1993, PageCount = 150, AuthorId = 1 },
            new() { Id = 2, Title = "Utopia", Year = 2008, PageCount = 192, AuthorId = 1 },
            new() { Id = 3, Title = "Palace Walk", Year = 1956, PageCount = 420, AuthorId = 2 },
            new() { Id = 4, Title = "Palace of Desire", Year = 1957, PageCount = 460, AuthorId = 2 },
            new() { Id = 5, Title = "Sugar Street", Year = 1957, PageCount = 380, AuthorId = 2 },
            new() { Id = 6, Title = "Dialogue with an Atheist Friend", Year = 1974, PageCount = 180, AuthorId = 3 },
            new() { Id = 7, Title = "My Journey from Doubt to Belief", Year = 1970, PageCount = 520, AuthorId = 3 },
            new() { Id = 8, Title = "The Spider", Year = 1965, PageCount = 210, AuthorId = 3 }
        });

        Tags.AddRange(new List<Tag>
        {
            new() { Id = 1, Name = "Horror" },
            new() { Id = 2, Name = "Novel" },
            new() { Id = 3, Name = "Philosophy" },
            new() { Id = 4, Name = "Fiction" }
        });

        BookTags.AddRange(new List<BookTag>
        {
            new() { BookId = 1, TagId = 1 },
            new() { BookId = 1, TagId = 2 },
            new() { BookId = 2, TagId = 2 },
            new() { BookId = 3, TagId = 2 },
            new() { BookId = 6, TagId = 3 },
            new() { BookId = 7, TagId = 3 }
        });

        foreach (var book in Books)
        {
            var author = Authors.FirstOrDefault(a => a.Id == book.AuthorId);
            if (author != null)
            {
                book.Author = author;
                author.Books.Add(book);
            }
        }
    }
}
