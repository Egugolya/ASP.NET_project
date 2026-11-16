using Lesson2.Models.Domain;
using Lesson2.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace Lesson2.Services;

public class CategoryService(global::Lesson2.Repositories.ICategoryRepository repository) : ICategoryService
{
    public Task<List<Category>> GetAllAsync() => repository.GetAllAsync();

    public Task<PaginatedResult<Category>> GetPaginatedAsync(PageResultRequest<CategoryFilter> request)
    {
        return repository.GetPaginatedAsync(
            request.Page,
            request.PageSize,
            GetApplyOrder(request.OrderBy),
            GetApplyFilter(request.Filter));
    }

    public Task<Category?> GetByIdAsync(int id) => repository.GetByIdAsync(id);

    public async Task<Category> CreateAsync(Category category)
    {
        var now = DateTimeOffset.UtcNow;
        category.Name = category.Name.Trim();
        category.CreatedAt = now;
        category.UpdatedAt = now;

        repository.Add(category);
        await repository.SaveChangesAsync();
        return category;
    }

    public async Task<Category?> UpdateAsync(int id, Category update)
    {
        var category = await repository.GetByIdAsync(id);
        if (category is null)
            return null;

        category.Name = update.Name.Trim();
        category.UpdatedAt = DateTimeOffset.UtcNow;
        await repository.SaveChangesAsync();
        return category;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var category = await repository.GetByIdAsync(id);
        if (category is null)
            return false;

        repository.Remove(category);
        await repository.SaveChangesAsync();
        return true;
    }

    private static Func<IQueryable<Category>, IQueryable<Category>?> GetApplyOrder(string? orderBy)
    {
        return orderBy?.ToLowerInvariant() switch
        {
            "id_asc" => query => query.OrderBy(category => category.Id),
            "name_desc" => query => query.OrderByDescending(category => category.Name).ThenBy(category => category.Id),
            "name_asc" => query => query.OrderBy(category => category.Name).ThenBy(category => category.Id),
            _ => query => query.OrderBy(category => category.Id)
        };
    }

    private static Func<IQueryable<Category>, IQueryable<Category>?>? GetApplyFilter(CategoryFilter? filter)
    {
        if (string.IsNullOrWhiteSpace(filter?.Name))
            return null;

        var name = filter.Name.Trim().ToLower();
        return query => query.Where(category => category.Name.ToLower() == name);
    }
}
