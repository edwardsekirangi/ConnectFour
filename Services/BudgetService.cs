using ConnectFour.Data;
using ConnectFour.Models;
using ConnectFour.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ConnectFour.Services;

public class BudgetService : IBudgetService
{
    private readonly ApplicationDbContext _dbContext;

    public BudgetService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<Budget>> GetBudgetsAsync(string userId)
    {
        return await _dbContext.Budgets
            .Include(b => b.Category)
            .Where(b => b.AppUserId == userId)
            .OrderBy(b => b.Year)
            .ThenBy(b => b.Month)
            .ToListAsync();
    }

    public async Task<Budget?> GetBudgetByIdAsync(int id, string userId)
    {
        return await _dbContext.Budgets
            .Include(b => b.Category)
            .FirstOrDefaultAsync(b => b.Id == id && b.AppUserId == userId);
    }

    public async Task AddBudgetAsync(Budget budget)
    {
        _dbContext.Budgets.Add(budget);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateBudgetAsync(Budget budget)
    {
        var existing = await _dbContext.Budgets
            .FirstOrDefaultAsync(b => b.Id == budget.Id && b.AppUserId == budget.AppUserId);

        if (existing is null)
        {
            throw new InvalidOperationException("Budget not found.");
        }

        existing.Limit = budget.Limit;
        existing.Month = budget.Month;
        existing.Year = budget.Year;
        existing.CategoryId = budget.CategoryId;

        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteBudgetAsync(int id, string userId)
    {
        var budget = await _dbContext.Budgets
            .FirstOrDefaultAsync(b => b.Id == id && b.AppUserId == userId);

        if (budget is null)
        {
            return;
        }

        _dbContext.Budgets.Remove(budget);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<decimal> GetRemainingBudgetAsync(string userId, int month, int year)
    {
        var totalIncome = await _dbContext.Transactions
            .Where(t => t.AppUserId == userId && t.Type == TransactionType.Income && t.Date.Month == month && t.Date.Year == year)
            .SumAsync(t => (decimal?)t.Amount) ?? 0m;

        var totalExpenses = await _dbContext.Transactions
            .Where(t => t.AppUserId == userId && t.Type == TransactionType.Expense && t.Date.Month == month && t.Date.Year == year)
            .SumAsync(t => (decimal?)t.Amount) ?? 0m;

        return totalIncome - totalExpenses;
    }

    public async Task<IEnumerable<CategoryBudgetProgress>> GetCategoryBudgetProgressAsync(string userId, int month, int year)
    {
        var budgets = await _dbContext.Budgets
            .Include(b => b.Category)
            .Where(b => b.AppUserId == userId && b.Month == month && b.Year == year)
            .ToListAsync();

        var result = new List<CategoryBudgetProgress>(budgets.Count);

        foreach (var budget in budgets)
        {
            var totalSpent = await _dbContext.Transactions
                .Where(t => t.AppUserId == userId && t.CategoryId == budget.CategoryId && t.Type == TransactionType.Expense && t.Date.Month == month && t.Date.Year == year)
                .SumAsync(t => (decimal?)t.Amount) ?? 0m;

            result.Add(new CategoryBudgetProgress
            {
                CategoryId = budget.CategoryId,
                CategoryName = budget.Category?.Name ?? string.Empty,
                BudgetLimit = budget.Limit,
                TotalSpent = totalSpent
            });
        }

        return result;
    }
}
