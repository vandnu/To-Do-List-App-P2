namespace ToDoListApp.DTOs
{
    public class TaskDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool IsCompleted { get; set; } // De vigtigste data?
    }
}