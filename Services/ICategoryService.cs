using Lesson2.Models.Domain;
using Lesson2.Models.DTO;

namespace Lesson2.Services;

public interface ICategoryService
{
    Task<List<Category>> GetAllAsync();
    Task<PaginatedResult<Category>> GetPaginatedAsync(PageResultRequest<CategoryFilter> request);
    Task<Category?> GetByIdAsync(int id);
    Task<Category> CreateAsync(Category category);
    Task<Category?> UpdateAsync(int id, Category update);
    Task<bool> DeleteAsync(int id);
}
