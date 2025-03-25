using System;
using System.ComponentModel.DataAnnotations;

namespace ToDoListApp.Models
{
    public class ToDoTask
    {
    public int Id { get; set; }
    [Required]
    public string Title { get; set; }
    public string Description { get; set; }
    [Required]
    public DateTime Deadline { get; set; }
    public bool IsCompleted { get; set; }
    public int UserId { get; set; }
    }
}