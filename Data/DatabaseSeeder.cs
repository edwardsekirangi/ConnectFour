using ConnectFour.Models;

namespace ConnectFour.Data;

public static class DatabaseSeeder
{
    public static async Task Seed(ApplicationDbContext context)
    {
        // Only seed if no data exists
        if (context.AppUsers.Any())
            return;

        // Create default user
        var defaultUser = new AppUser
        {
            Id = "default-user-id",
            UserName = "DefaultUser",
            Email = "user@example.com"
        };

        context.AppUsers.Add(defaultUser);
        await context.SaveChangesAsync();

        // Create categories
        var categories = new List<Category>
        {
            new Category { Name = "Food", AppUserId = defaultUser.Id },
            new Category { Name = "Transport", AppUserId = defaultUser.Id },
            new Category { Name = "Entertainment", AppUserId = defaultUser.Id },
            new Category { Name = "Utilities", AppUserId = defaultUser.Id },
            new Category { Name = "Healthcare", AppUserId = defaultUser.Id },
            new Category { Name = "Salary", AppUserId = defaultUser.Id },
            new Category { Name = "Freelance", AppUserId = defaultUser.Id },
            new Category { Name = "Other", AppUserId = defaultUser.Id }
        };

        context.Categories.AddRange(categories);
        await context.SaveChangesAsync();

        // Create sample transactions
        var transactions = new List<Transaction>
        {
            new Transaction
            {
                Amount = 50.00m,
                Date = DateTime.Now.AddDays(-10),
                Description = "Grocery shopping",
                Type = TransactionType.Expense,
                CategoryId = categories.First(c => c.Name == "Food").Id,
                AppUserId = defaultUser.Id
            },
            new Transaction
            {
                Amount = 2500.00m,
                Date = DateTime.Now.AddDays(-5),
                Description = "Monthly salary",
                Type = TransactionType.Income,
                CategoryId = categories.First(c => c.Name == "Salary").Id,
                AppUserId = defaultUser.Id
            },
            new Transaction
            {
                Amount = 30.00m,
                Date = DateTime.Now.AddDays(-3),
                Description = "Taxi fare",
                Type = TransactionType.Expense,
                CategoryId = categories.First(c => c.Name == "Transport").Id,
                AppUserId = defaultUser.Id
            },
            new Transaction
            {
                Amount = 15.00m,
                Date = DateTime.Now.AddDays(-2),
                Description = "Movie tickets",
                Type = TransactionType.Expense,
                CategoryId = categories.First(c => c.Name == "Entertainment").Id,
                AppUserId = defaultUser.Id
            },
            new Transaction
            {
                Amount = 80.00m,
                Date = DateTime.Now.AddDays(-1),
                Description = "Electricity bill",
                Type = TransactionType.Expense,
                CategoryId = categories.First(c => c.Name == "Utilities").Id,
                AppUserId = defaultUser.Id
            },
            new Transaction
            {
                Amount = 45.00m,
                Date = DateTime.Now,
                Description = "Restaurant dinner",
                Type = TransactionType.Expense,
                CategoryId = categories.First(c => c.Name == "Food").Id,
                AppUserId = defaultUser.Id
            }
        };

        context.Transactions.AddRange(transactions);
        await context.SaveChangesAsync();

        // Create budgets
        var budgets = new List<Budget>
        {
            new Budget
            {
                Limit = 300.00m,
                Month = DateTime.Now.Month,
                Year = DateTime.Now.Year,
                CategoryId = categories.First(c => c.Name == "Food").Id,
                AppUserId = defaultUser.Id
            },
            new Budget
            {
                Limit = 200.00m,
                Month = DateTime.Now.Month,
                Year = DateTime.Now.Year,
                CategoryId = categories.First(c => c.Name == "Transport").Id,
                AppUserId = defaultUser.Id
            },
            new Budget
            {
                Limit = 150.00m,
                Month = DateTime.Now.Month,
                Year = DateTime.Now.Year,
                CategoryId = categories.First(c => c.Name == "Entertainment").Id,
                AppUserId = defaultUser.Id
            },
            new Budget
            {
                Limit = 200.00m,
                Month = DateTime.Now.Month,
                Year = DateTime.Now.Year,
                CategoryId = categories.First(c => c.Name == "Utilities").Id,
                AppUserId = defaultUser.Id
            },
            new Budget
            {
                Limit = 300.00m,
                Month = DateTime.Now.Month,
                Year = DateTime.Now.Year,
                CategoryId = categories.First(c => c.Name == "Healthcare").Id,
                AppUserId = defaultUser.Id
            }
        };

        context.Budgets.AddRange(budgets);
        await context.SaveChangesAsync();
    }
}
