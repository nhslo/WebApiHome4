using System.ComponentModel.DataAnnotations;

namespace WebApiHome4.DTOs;

public class CreateBookDto
{
    [Required, StringLength(160, MinimumLength = 1)]
    public string Title { get; set; } = string.Empty;

    [Required, StringLength(120, MinimumLength = 2)]
    public string Author { get; set; } = string.Empty;

    [Range(1450, 2100)]
    public int Year { get; set; }

    [Range(typeof(decimal), "1", "100000000")]
    public decimal Price { get; set; }

    [StringLength(2000)]
    public string Description { get; set; } = string.Empty;
}
