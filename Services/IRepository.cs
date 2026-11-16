using Lesson2.Models.DTO;

namespace Lesson2.Services
{
    public interface IRepository<T> where T : class
    {
        Task<ICollection<T>> GetAllAsync();
        Task<PaginatedResult<T>> GetPaginatedAsync(
            int page,
            int pageSize,
            Func<IQueryable<T>, IQueryable<T>?>? orderBy = null,
            Func<IQueryable<T>, IQueryable<T>?>? filter = null);
        Task<T?> GetByIdAsync(string id);
        void Add(T entity);
        void Remove(T entity);
        Task SaveChangesAsync();
    }
}
