using Microsoft.EntityFrameworkCore;

namespace Lesson2.Models
{
    public class ApiDbContext : DbContext
    {
        public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options)
        {
        }

        public virtual DbSet<Sender> Senders { get; set; }
        public virtual DbSet<Message> Messages { get; set; }
        public virtual DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Sender>().HasKey(sender => sender.Email);
            base.OnModelCreating(modelBuilder);
        }
    }
}
