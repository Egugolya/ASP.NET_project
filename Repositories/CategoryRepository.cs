using Lesson2.Models;
using Lesson2.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Lesson2.Repositories;

public class CategoryRepository(ApiDbContext context) : BaseRepository<Category>(context), ICategoryRepository
{
    public override Task<List<Category>> GetAllAsync() => Entities
        .AsNoTracking()
        .OrderBy(category => category.Id)
        .ToListAsync();

    public Task<Category?> GetByIdAsync(int id) => FindAsync(id).AsTask();
}
