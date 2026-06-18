namespace ConnectFour.Models;

public class AppUser
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string UserName { get; set; } = "DefaultUser";
    public string Email { get; set; } = "user@example.com";

    public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    public ICollection<Category> Categories { get; set; } = new List<Category>();
    public ICollection<Budget> Budgets { get; set; } = new List<Budget>();
}
