using Microsoft.EntityFrameworkCore;
using ToDoListApp.Models;

namespace ToDoListApp.Models
{
    public class AppDbContext : DbContext
    {
        public DbSet<ToDoTask> ToDoTasks { get; set; }
        public DbSet<Quote> Quotes { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }   

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
}
}