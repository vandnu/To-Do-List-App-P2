namespace ToDoListApp.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; } // For kryptering
        public string Role { get; set; } // "User" eller "Admin"
        public string Email { get; set;}
        public DateTime CreatedTime { get; set; }

        //ToDoTasks for specifik bruger

        public List<ToDoTask> ToDoTasks { get; set;} = new List<ToDoTask>();
    }
}