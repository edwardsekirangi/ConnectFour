using ConnectFour.Services.Interfaces;

namespace ConnectFour.Services.Interfaces;

public interface IDashboardService
{
    Task<decimal> GetTotalIncomeForCurrentMonthAsync(string userId);
    Task<decimal> GetTotalExpensesForCurrentMonthAsync(string userId);
    Task<decimal> GetRemainingBudgetForCurrentMonthAsync(string userId);
}
