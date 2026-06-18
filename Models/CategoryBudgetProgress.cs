namespace ConnectFour.Models;

public class CategoryBudgetProgress
{
    public int CategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public decimal BudgetLimit { get; set; }
    public decimal TotalSpent { get; set; }

    public decimal PercentSpent => BudgetLimit <= 0m
        ? 0m
        : Math.Round((TotalSpent / BudgetLimit) * 100m, 2);
}
