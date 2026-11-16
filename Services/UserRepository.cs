using Microsoft.EntityFrameworkCore;
using Lesson2.Models;
using Lesson2.Models.DTO;

namespace Lesson2.Services
{ 
    public class UserRepository(ApiDbContext context) : IRepository<Sender>
    {
        public async Task<ICollection<Sender>> GetAllAsync()
        {
            return await context.Senders.AsNoTracking().ToListAsync();
        }
        public async Task<PaginatedResult<Sender>> GetPaginatedAsync(
            int page,
            int pageSize,
            Func<IQueryable<Sender>, IQueryable<Sender>?>? orderBy = null,
            Func<IQueryable<Sender>, IQueryable<Sender>?>? filter = null)
        {
            if (page < 1)
                throw new ArgumentOutOfRangeException(nameof(page));
            if (pageSize is < 1 or > 100)
                throw new ArgumentOutOfRangeException(nameof(pageSize));

            IQueryable<Sender> query = context.Senders.AsNoTracking();
            query = filter?.Invoke(query) ?? query;
            var totalCount = await query.CountAsync();
            var orderedQuery = orderBy?.Invoke(query) ?? query;
            var items = await orderedQuery.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            return new PaginatedResult<Sender>
            {
                Models = items,
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount
            };
        }
        public async Task<Sender?> GetByIdAsync(string id)
        {
            return await context.Senders.FindAsync(id);
        }
        public void Add(Sender entity)
        {
            context.Senders.Add(entity);
        }
        public void Remove(Sender entity)
        {
            context.Senders.Remove(entity);
        }
        public async Task SaveChangesAsync()
        {
            await context.SaveChangesAsync();
        }
    }
}