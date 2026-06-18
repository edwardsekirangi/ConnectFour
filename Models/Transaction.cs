using System.ComponentModel.DataAnnotations;

namespace ConnectFour.Models;

public class Transaction
{
    public int Id { get; set; }

    [Required]
    [Range(0.01, double.MaxValue)]
    public decimal Amount { get; set; }

    public DateTime Date { get; set; }

    [Required]
    [MaxLength(500)]
    public string Description { get; set; } = string.Empty;

    public TransactionType Type { get; set; }

    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;

    public string AppUserId { get; set; } = string.Empty;
    public AppUser AppUser { get; set; } = null!;
}
