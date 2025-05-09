using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace api.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string PasswordHash { get; set; } // For kryptering
        [Required]
        public string Role { get; set; } // "User" eller "Admin"
        [Required]
        public string Email { get; set;}
        public DateTime CreatedTime { get; set; }

        //ToDoTasks for specifik bruger

        public List<ToDoTask> ToDoTasks { get; set;} = new List<ToDoTask>();
    }
}