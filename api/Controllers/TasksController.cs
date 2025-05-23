using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using api.Models;
using api.DTOs;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToDoTasksController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ToDoTasksController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/ToDoTasks/my-tasks - Henter brugerens opgaver
        [Authorize]
        [HttpGet("my-tasks")]
        public async Task<ActionResult<IEnumerable<TaskDto>>> GetMyTasks()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var tasks = await _context.ToDoTasks
                .Where(t => t.UserId == userId)
                .Select(t => new TaskDto
                {
                    Id = t.Id,
                    Title = t.Title,
                    IsCompleted = t.IsCompleted,
                    Description = t.Description,
                    UserId = t.UserId
                })
                .ToListAsync();

            return Ok(tasks);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("all-tasks")]
        public async Task<ActionResult<IEnumerable<TaskDto>>> GetAllTasks()
        {
            var tasks = await _context.ToDoTasks
                .Select(t => new TaskDto
                {
                    Id = t.Id,
                    Title = t.Title,
                    IsCompleted = t.IsCompleted,
                    Description = t.Description,
                    UserId = t.UserId
                })
                .ToListAsync();

            return Ok(tasks);
        }

        // GET: api/ToDoTasks/8 - Henter en specifik opgave baseret på id
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<TaskDto>> GetToDoTask(int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var todoTask = await _context.ToDoTasks
                .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

            if (todoTask == null)
            {
                return Forbid("You do not have permission to access this task.");
            }

            var taskDto = new TaskDto
            {
                Id = todoTask.Id,
                Title = todoTask.Title,
                IsCompleted = todoTask.IsCompleted,
                Description = todoTask.Description
            };

            return Ok(taskDto);
        }

        // POST: api/ToDoTasks - Opretter en ny opgave
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<TaskDto>> PostToDoTask(TaskDto taskDto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var todoTask = new ToDoTask
            {
                Title = taskDto.Title,
                Description = taskDto.Description,
                IsCompleted = taskDto.IsCompleted,
                UserId = userId
            };

            _context.ToDoTasks.Add(todoTask);
            await _context.SaveChangesAsync();

            taskDto.Id = todoTask.Id;
            return CreatedAtAction(nameof(GetToDoTask), new { id = todoTask.Id }, taskDto);
        }

        // PUT: api/ToDoTasks/5 - Opdaterer en eksisterende opgave
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateTask(int id, [FromBody] ToDoTask updatedTask)
        {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

        ToDoTask? todoTask = userRole == "Admin"
            ? await _context.ToDoTasks.FirstOrDefaultAsync(t => t.Id == id)
            : await _context.ToDoTasks.FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

        if (todoTask == null)
            return NotFound("Task not found or you don't have permission to update it.");

        // Opdater kun de felter, der er sendt
        if (updatedTask.Title != null) todoTask.Title = updatedTask.Title;
        if (updatedTask.Description != null) todoTask.Description = updatedTask.Description;
        todoTask.IsCompleted = updatedTask.IsCompleted; // IsCompleted er påkrævet

        await _context.SaveChangesAsync();
        return Ok(todoTask);
        }

        [HttpPatch("{id}/complete")]
        [Authorize]
        public async Task<IActionResult> UpdateTaskCompletion(int id, [FromBody] bool isCompleted)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

            ToDoTask? todoTask = userRole == "Admin"
                ? await _context.ToDoTasks.FirstOrDefaultAsync(t => t.Id == id)
                : await _context.ToDoTasks.FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

            if (todoTask == null)
                return NotFound("Task not found or you don't have permission to update it.");

            todoTask.IsCompleted = isCompleted;
            await _context.SaveChangesAsync();
            return Ok(todoTask);
        }


        // DELETE: api/ToDoTasks/5 - Sletter en opgave
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteTask(int id)
        {
            // Hent brugerens ID og rolle fra JWT-tokenet
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

            // Find opgaven
            ToDoTask? todoTask;

            if (userRole == "Admin")
            {
                // Admin kan slette alle opgaver
                todoTask = await _context.ToDoTasks.FirstOrDefaultAsync(t => t.Id == id);
            }
            else
            {
                // Almindelige brugere kan kun slette deres egne opgaver
                todoTask = await _context.ToDoTasks.FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);
            }

            // Tjek, om opgaven findes
            if (todoTask == null)
            {
                return NotFound("Task not found or you don't have permission to delete it.");
            }

            // Slet opgaven
            _context.ToDoTasks.Remove(todoTask);
            await _context.SaveChangesAsync();

            return Ok("Task deleted successfully.");
        }

        private bool ToDoTaskExists(int id)
        {
            return _context.ToDoTasks.Any(e => e.Id == id);
        }
    }
}