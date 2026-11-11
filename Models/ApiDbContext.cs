using Microsoft.EntityFrameworkCore;

namespace Lesson2.Models
{
    public class ApiDbContext : DbContext
    {
        public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options)
        {
        }

        public virtual DbSet<Sender> Senders { get; set; }
        public virtual DbSet<SenderToken> SenderTokens { get; set; }
        public virtual DbSet<Message> Messages { get; set; }
        public virtual DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Sender>().HasKey(sender => sender.Email);
            modelBuilder.Entity<SenderToken>().HasKey(token => token.TokenHash);
            modelBuilder.Entity<SenderToken>()
                .HasOne(token => token.Sender)
                .WithMany()
                .HasForeignKey(token => token.SenderEmail)
                .OnDelete(DeleteBehavior.Cascade);
            base.OnModelCreating(modelBuilder);
        }
    }
}
