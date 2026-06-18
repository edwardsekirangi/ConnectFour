using System.ComponentModel.DataAnnotations;

namespace ConnectFour.Models;

public class Budget
{
    public int Id { get; set; }

    [Required]
    [Range(0, double.MaxValue)]
    public decimal Limit { get; set; }

    [Range(1, 12)]
    public int Month { get; set; }

    public int Year { get; set; }

    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;

    public string AppUserId { get; set; } = string.Empty;
    public AppUser AppUser { get; set; } = null!;
}
