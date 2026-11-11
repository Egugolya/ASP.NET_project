using Microsoft.EntityFrameworkCore;
using Lesson2.Models;

namespace Lesson2.Services
{ 
    public class UserRepository(ApiDbContext context) : IRepository<Sender>
    {
        public async Task<ICollection<Sender>> GetAllAsync()
        {
            return await context.Senders.ToListAsync();
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