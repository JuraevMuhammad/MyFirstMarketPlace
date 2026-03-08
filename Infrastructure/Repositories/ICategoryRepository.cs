using Domain.Entities;

namespace Infrastructure.Repositories;

public interface ICategoryRepository
{
    Task<int> CreateCategoryAsync(Category category);
    Task<List<Category>?> GetCategoriesAsync();
    Task<Category?> GetCategoryAsync(int id);
    Task<int> DeleteSaveAsync(int id);
    Task<int> UpdateSaveAsync(Category category);
}