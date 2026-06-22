using System.ComponentModel.DataAnnotations;

namespace tasks.DTOs;

public class BookUpdateDTO
{
    [Required(ErrorMessage = "Title is required.")]
    public string Title { get; set; } = string.Empty;

    [Range(1, int.MaxValue, ErrorMessage = "Author ID must be a positive integer.")]
    public int AuthorId { get; set; }

    [Range(1, 9999, ErrorMessage = "Year must be a valid year.")]
    public int Year { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Page count must be at least 1.")]
    public int PageCount { get; set; }
}
