using ConnectFour.Models;

namespace ConnectFour.Services.Interfaces;

public interface ITransactionService
{
    Task<IEnumerable<Transaction>> GetTransactionsAsync(string userId);
    Task<Transaction?> GetTransactionByIdAsync(int id, string userId);
    Task AddTransactionAsync(Transaction transaction);
    Task UpdateTransactionAsync(Transaction transaction);
    Task DeleteTransactionAsync(int id, string userId);
    Task<decimal> GetTotalIncomeAsync(string userId, int month, int year);
    Task<decimal> GetTotalExpensesAsync(string userId, int month, int year);
    Task<IEnumerable<Transaction>> GetTransactionsByCategoryAsync(int categoryId, string userId, int month, int year);
    Task<Dictionary<string, decimal>> GetExpenseTotalsByCategoryAsync(string userId, int month, int year);
}
