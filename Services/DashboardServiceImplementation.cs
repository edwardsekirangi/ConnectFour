using ConnectFour.Services.Interfaces;

namespace ConnectFour.Services;

public class DashboardService : IDashboardService
{
    private readonly ITransactionService _transactionService;

    public DashboardService(ITransactionService transactionService)
    {
        _transactionService = transactionService;
    }

    public async Task<decimal> GetTotalIncomeForCurrentMonthAsync(string userId)
    {
        var now = DateTime.UtcNow;
        return await _transactionService.GetTotalIncomeAsync(userId, now.Month, now.Year);
    }

    public async Task<decimal> GetTotalExpensesForCurrentMonthAsync(string userId)
    {
        var now = DateTime.UtcNow;
        return await _transactionService.GetTotalExpensesAsync(userId, now.Month, now.Year);
    }

    public async Task<decimal> GetRemainingBudgetForCurrentMonthAsync(string userId)
    {
        var now = DateTime.UtcNow;
        return await _transactionService.GetTotalIncomeAsync(userId, now.Month, now.Year)
            - await _transactionService.GetTotalExpensesAsync(userId, now.Month, now.Year);
    }
}
