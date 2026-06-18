using ConnectFour.Data;
using ConnectFour.Models;
using ConnectFour.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ConnectFour.Services;

public class TransactionService : ITransactionService
{
    private readonly ApplicationDbContext _dbContext;

    public TransactionService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<Transaction>> GetTransactionsAsync(string userId)
    {
        return await _dbContext.Transactions
            .Include(t => t.Category)
            .Where(t => t.AppUserId == userId)
            .OrderByDescending(t => t.Date)
            .ToListAsync();
    }

    public async Task<Transaction?> GetTransactionByIdAsync(int id, string userId)
    {
        return await _dbContext.Transactions
            .Include(t => t.Category)
            .FirstOrDefaultAsync(t => t.Id == id && t.AppUserId == userId);
    }

    public async Task AddTransactionAsync(Transaction transaction)
    {
        _dbContext.Transactions.Add(transaction);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateTransactionAsync(Transaction transaction)
    {
        var existing = await _dbContext.Transactions
            .FirstOrDefaultAsync(t => t.Id == transaction.Id && t.AppUserId == transaction.AppUserId);

        if (existing is null)
        {
            throw new InvalidOperationException("Transaction not found.");
        }

        existing.Amount = transaction.Amount;
        existing.Date = transaction.Date;
        existing.Description = transaction.Description;
        existing.Type = transaction.Type;
        existing.CategoryId = transaction.CategoryId;

        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteTransactionAsync(int id, string userId)
    {
        var transaction = await _dbContext.Transactions
            .FirstOrDefaultAsync(t => t.Id == id && t.AppUserId == userId);

        if (transaction is null)
        {
            return;
        }

        _dbContext.Transactions.Remove(transaction);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<decimal> GetTotalIncomeAsync(string userId, int month, int year)
    {
        return await _dbContext.Transactions
            .Where(t => t.AppUserId == userId && t.Type == TransactionType.Income && t.Date.Month == month && t.Date.Year == year)
            .SumAsync(t => (decimal?)t.Amount) ?? 0m;
    }

    public async Task<decimal> GetTotalExpensesAsync(string userId, int month, int year)
    {
        return await _dbContext.Transactions
            .Where(t => t.AppUserId == userId && t.Type == TransactionType.Expense && t.Date.Month == month && t.Date.Year == year)
            .SumAsync(t => (decimal?)t.Amount) ?? 0m;
    }

    public async Task<IEnumerable<Transaction>> GetTransactionsByCategoryAsync(int categoryId, string userId, int month, int year)
    {
        return await _dbContext.Transactions
            .Include(t => t.Category)
            .Where(t => t.AppUserId == userId && t.CategoryId == categoryId && t.Date.Month == month && t.Date.Year == year)
            .OrderByDescending(t => t.Date)
            .ToListAsync();
    }

    public async Task<Dictionary<string, decimal>> GetExpenseTotalsByCategoryAsync(string userId, int month, int year)
    {
        return await _dbContext.Transactions
            .Where(t => t.AppUserId == userId && t.Type == TransactionType.Expense && t.Date.Month == month && t.Date.Year == year)
            .GroupBy(t => t.Category.Name)
            .Select(g => new { CategoryName = g.Key, Total = g.Sum(t => t.Amount) })
            .ToDictionaryAsync(g => g.CategoryName, g => g.Total);
    }
}
