using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ToDoListApp.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        // "Definerer" DbSets for tabeller
        public DbSet<ToDoTask> ToDoTasks { get; set; }
        public DbSet<Quote> Quotes { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Relationen mellem ToDoTask og User
            modelBuilder.Entity<ToDoTask>()
                .HasOne(t => t.User)          // En ToDoTask har én User
                .WithMany(u => u.ToDoTasks)   // En User kan have mange ToDoTasks
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Cascase); // Sletter alt hvad en bruger har af tasks ved sletning af bruger


            // Seeding af data
            modelBuilder.Entity<Quote>()
                .HasData(
                    new Quote { Id = 1, Text = "Gør det i dag - det som du ellers kan overveje at udsætte til i morgen!", Author = "V"},
                    new Quote { Id = 2, Text = "Små steps/sejre hver dag (mod et større mål) kan føre til store resultater.", Author = "A"}
                );

            // "Seeding?" af admin-bruger
            modelBuilder.Entity<User>()
                .HasData(
                    new User
                    {
                        Id = 1,
                        Username = "admin",
                        PasswordHash = "$2a$11$IsepszmC6snjPcfCcAmo5uctrPqXcLzRRoEVkpywIOw2kG7T./fOu", //Hashet adminKode
                        Role = "Admin",
                        Email = "test@ellernoget.com",
                        CreatedAt = DateTime.UtcNow
                    }
                );

            
            // Eventuelle yderligere konfigurationer her - f.eks. andre relationer
            // Eksempel: modelBuilder.Entity<ToDoTask>().HasKey(t => t.Id);
        }
    }
}
