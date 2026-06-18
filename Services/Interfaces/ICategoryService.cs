using ConnectFour.Models;

namespace ConnectFour.Services.Interfaces;

public interface ICategoryService
{
    Task<IEnumerable<Category>> GetCategoriesAsync(string userId);
    Task<Category?> GetCategoryByIdAsync(int categoryId, string userId);
    Task AddCategoryAsync(Category category);
}
