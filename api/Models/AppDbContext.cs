using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace api.Models
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
                .OnDelete(DeleteBehavior.Cascade); // Sletter alt hvad en bruger har af tasks ved sletning af bruger

            // Gør Description valgfri
            modelBuilder.Entity<ToDoTask>()
                .Property(t => t.Description)
                .IsRequired(false);

            // Seeding af data
            modelBuilder.Entity<Quote>()
                .HasData(
                    new Quote { Id = 1, Text = "Gør det i dag - det som du ellers kan overveje at udsætte til i morgen!", Author = "V"},
                    new Quote { Id = 2, Text = "Små steps/sejre hver dag (mod et større mål) kan føre til store resultater.", Author = "A"},
                
                    new Quote { Id = 3, Text = "The only way to do great work is to love what you do.", Author = "Steve Jobs"},
                    new Quote { Id = 4, Text = "Strive not to be a success, but rather to be of value.", Author = "Albert Einstein"},
                    new Quote { Id = 5, Text = "Your time is limited, so don’t waste it living someone else’s life.", Author = "Steve Jobs"},
                    new Quote { Id = 6, Text = "The mind is everything. What you think you become.", Author = "Buddha"},
                    new Quote { Id = 7, Text = "The best way to predict the future is to create it.", Author = "Peter Drucker"},
                    new Quote { Id = 8, Text = "Success is not final, failure is not fatal: It is the courage to continue that counts.", Author = "Winston Churchill"},
                    new Quote { Id = 9, Text = "Believe you can and you're halfway there.", Author = "Theodore Roosevelt"},
                    new Quote { Id = 10, Text = "Act as if what you do makes a difference. It does.", Author = "William James"},
                    new Quote { Id = 11, Text = "It does not matter how slowly you go as long as you do not stop.", Author = "Confucius"},
                    new Quote { Id = 12, Text = "The future belongs to those who believe in the beauty of their dreams.", Author = "Eleanor Roosevelt"},
                    new Quote { Id = 13, Text = "Do not wait to strike till the iron is hot; but make it hot by striking.", Author = "William Butler Yeats"},
                    new Quote { Id = 14, Text = "Keep your face always toward the sunshine, and shadows will fall behind you.", Author = "Walt Whitman"},
                    new Quote { Id = 15, Text = "What lies behind us and what lies before us are tiny matters compared to what lies within us.", Author = "Ralph Waldo Emerson"}
                );

            // Seeding af admin-bruger
            modelBuilder.Entity<User>()
                .HasData(
                    new User
                    {
                        Id = 1,
                        Username = "admin",
                        PasswordHash = "$2a$11$IsepszmC6snjPcfCcAmo5uctrPqXcLzRRoEVkpywIOw2kG7T./fOu", //Hashet adminKode
                        Role = "Admin",
                        Email = "test@ellernoget.com",
                        CreatedTime = new DateTime(2025, 4, 29, 12, 0, 0, DateTimeKind.Utc),
                        favoriteMotivator = null
                    }
                );

            
            // Eventuelle yderligere konfigurationer her - f.eks. andre relationer
            // Eksempel: modelBuilder.Entity<ToDoTask>().HasKey(t => t.Id);
        }
    }
}
