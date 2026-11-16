using Lesson2.Models;
using Lesson2.Models.Domain;
using Lesson2.Models.DTO;

namespace Lesson2.Repositories;

public interface ICategoryRepository
{
    Task<List<Category>> GetAllAsync();
    Task<PaginatedResult<Category>> GetPaginatedAsync(
        int page,
        int pageSize,
        Func<IQueryable<Category>, IQueryable<Category>?>? orderBy = null,
        Func<IQueryable<Category>, IQueryable<Category>?>? filter = null);
    Task<Category?> GetByIdAsync(int id);
    void Add(Category category);
    void Remove(Category category);
    Task SaveChangesAsync();
}
