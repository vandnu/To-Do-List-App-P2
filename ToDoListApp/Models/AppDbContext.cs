using Microsoft.EntityFrameworkCore;

namespace ToDoListApp.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        // "Definerer" DbSets for dine tabeller
        public DbSet<ToDoTask> ToDoTasks { get; set; }
        public DbSet<Quote> Quotes { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Eventuelle yderligere konfigurationer her - f.eks. relationer
            // Eksempel: modelBuilder.Entity<ToDoTask>().HasKey(t => t.Id);
        }
    }
}
