using Microsoft.AspNetCore.Identity;

namespace ConnectFour.Models;

public class AppUser : IdentityUser
{
    public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    public ICollection<Category> Categories { get; set; } = new List<Category>();
    public ICollection<Budget> Budgets { get; set; } = new List<Budget>();
}
