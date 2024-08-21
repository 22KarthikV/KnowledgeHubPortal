using KnowledgeHubPortal.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace KnowledgeHubPortal.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Url> Urls { get; set; }
        public DbSet<Statistic> Statistics { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Url>()
            .HasOne(u => u.Category)
            .WithMany()
            .HasForeignKey(u => u.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Url>()
                .HasOne(u => u.User)
                .WithMany()
                .HasForeignKey(u => u.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}