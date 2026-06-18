using ConnectFour.Models;

namespace ConnectFour.Services.Interfaces;

public interface IBudgetService
{
    Task<IEnumerable<Budget>> GetBudgetsAsync(string userId);
    Task<Budget?> GetBudgetByIdAsync(int id, string userId);
    Task AddBudgetAsync(Budget budget);
    Task UpdateBudgetAsync(Budget budget);
    Task DeleteBudgetAsync(int id, string userId);
    Task<decimal> GetRemainingBudgetAsync(string userId, int month, int year);
    Task<IEnumerable<CategoryBudgetProgress>> GetCategoryBudgetProgressAsync(string userId, int month, int year);
}
