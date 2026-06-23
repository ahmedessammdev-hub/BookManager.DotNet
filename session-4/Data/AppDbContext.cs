using Microsoft.EntityFrameworkCore;
using tasks.Entities;

namespace tasks.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Book> Books => Set<Book>();
    public DbSet<Author> Authors => Set<Author>();
    public DbSet<Tag> Tags => Set<Tag>();
    public DbSet<BookTag> BookTags => Set<BookTag>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<BookTag>()
            .HasKey(bt => new { bt.BookId, bt.TagId });

        modelBuilder.Entity<BookTag>()
            .HasOne<Book>()
            .WithMany()
            .HasForeignKey(bt => bt.BookId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<BookTag>()
            .HasOne<Tag>()
            .WithMany()
            .HasForeignKey(bt => bt.TagId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Author>().HasData(
            new Author { Id = 1, Name = "Ahmed Khaled Tawfik" },
            new Author { Id = 2, Name = "Naguib Mahfouz" },
            new Author { Id = 3, Name = "Mustafa Mahmoud" }
        );

        modelBuilder.Entity<Book>().HasData(
            new Book { Id = 1, Title = "Paranormal", Year = 1993, PageCount = 150, AuthorId = 1 },
            new Book { Id = 2, Title = "Utopia", Year = 2008, PageCount = 192, AuthorId = 1 },
            new Book { Id = 3, Title = "Palace Walk", Year = 1956, PageCount = 420, AuthorId = 2 },
            new Book { Id = 4, Title = "Palace of Desire", Year = 1957, PageCount = 460, AuthorId = 2 },
            new Book { Id = 5, Title = "Sugar Street", Year = 1957, PageCount = 380, AuthorId = 2 },
            new Book { Id = 6, Title = "Dialogue with an Atheist Friend", Year = 1974, PageCount = 180, AuthorId = 3 },
            new Book { Id = 7, Title = "My Journey from Doubt to Belief", Year = 1970, PageCount = 520, AuthorId = 3 },
            new Book { Id = 8, Title = "The Spider", Year = 1965, PageCount = 210, AuthorId = 3 }
        );

        modelBuilder.Entity<Tag>().HasData(
            new Tag { Id = 1, Name = "Horror" },
            new Tag { Id = 2, Name = "Novel" },
            new Tag { Id = 3, Name = "Philosophy" },
            new Tag { Id = 4, Name = "Fiction" }
        );

        modelBuilder.Entity<BookTag>().HasData(
            new BookTag { BookId = 1, TagId = 1 },
            new BookTag { BookId = 1, TagId = 2 },
            new BookTag { BookId = 2, TagId = 2 },
            new BookTag { BookId = 3, TagId = 2 },
            new BookTag { BookId = 6, TagId = 3 },
            new BookTag { BookId = 7, TagId = 3 }
        );
    }
}
