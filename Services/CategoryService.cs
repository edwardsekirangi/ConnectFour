using ConnectFour.Data;
using ConnectFour.Models;
using ConnectFour.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ConnectFour.Services;

public class CategoryService : ICategoryService
{
    private readonly ApplicationDbContext _dbContext;

    public CategoryService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<Category>> GetCategoriesAsync(string userId)
    {
        return await _dbContext.Categories
            .Where(c => c.AppUserId == userId)
            .OrderBy(c => c.Name)
            .ToListAsync();
    }

    public async Task<Category?> GetCategoryByIdAsync(int categoryId, string userId)
    {
        return await _dbContext.Categories
            .FirstOrDefaultAsync(c => c.Id == categoryId && c.AppUserId == userId);
    }

    public async Task AddCategoryAsync(Category category)
    {
        _dbContext.Categories.Add(category);
        await _dbContext.SaveChangesAsync();
    }
}
