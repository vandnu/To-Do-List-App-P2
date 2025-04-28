using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace ToDoListApp.Models
{
    public class Quote
    {
    public int Id { get; set; }
    [Required]
    public string Text { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    }
}