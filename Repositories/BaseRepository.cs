using Lesson2.Models;
using Lesson2.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace Lesson2.Repositories;

public abstract class BaseRepository<TEntity>(ApiDbContext context) where TEntity : class
{
    protected DbSet<TEntity> Entities => context.Set<TEntity>();

    public virtual Task<List<TEntity>> GetAllAsync() => Entities.AsNoTracking().ToListAsync();

    public virtual async Task<PaginatedResult<TEntity>> GetPaginatedAsync(
        int page,
        int pageSize,
        Func<IQueryable<TEntity>, IQueryable<TEntity>?>? orderBy = null,
        Func<IQueryable<TEntity>, IQueryable<TEntity>?>? filter = null)
    {
        ValidatePage(page, pageSize);

        IQueryable<TEntity> query = Entities.AsNoTracking();
        query = filter?.Invoke(query) ?? query;
        var totalCount = await query.CountAsync();
        var orderedQuery = orderBy?.Invoke(query) ?? query;
        var items = await orderedQuery.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

        return new PaginatedResult<TEntity>
        {
            Models = items,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }

    public virtual void Add(TEntity entity) => Entities.Add(entity);

    public virtual void Remove(TEntity entity) => Entities.Remove(entity);

    public virtual Task SaveChangesAsync() => context.SaveChangesAsync();

    protected ValueTask<TEntity?> FindAsync(params object?[] keyValues) => Entities.FindAsync(keyValues);

    protected static void ValidatePage(int page, int pageSize)
    {
        if (page < 1)
            throw new ArgumentOutOfRangeException(nameof(page));
        if (pageSize is < 1 or > 100)
            throw new ArgumentOutOfRangeException(nameof(pageSize));
    }
}
