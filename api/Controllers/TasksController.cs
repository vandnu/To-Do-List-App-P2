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
                    IsCompleted = t.IsCompleted
                })
                .ToListAsync();

            return Ok(tasks);
        }

        // GET: api/ToDoTasks/all-tasks - Henter alle opgaver (kun for admins)
        [Authorize(Roles = "Admin")]
        [HttpGet("all-tasks")]
        public async Task<ActionResult<IEnumerable<TaskDto>>> GetAllTasks()
        {
            var tasks = await _context.ToDoTasks
                .Select(t => new TaskDto
                {
                    Id = t.Id,
                    Title = t.Title,
                    IsCompleted = t.IsCompleted
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
                IsCompleted = todoTask.IsCompleted
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
                IsCompleted = taskDto.IsCompleted,
                UserId = userId
            };

            _context.ToDoTasks.Add(todoTask);
            await _context.SaveChangesAsync();

            taskDto.Id = todoTask.Id;
            return CreatedAtAction(nameof(GetToDoTask), new { id = todoTask.Id }, taskDto);
        }

        // PUT: api/ToDoTasks/5 - Opdaterer en eksisterende opgave
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutToDoTask(int id, TaskDto taskDto)
        {
            if (id != taskDto.Id) return BadRequest();

            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var todoTask = await _context.ToDoTasks
                .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

            if (todoTask == null) return NotFound();

            todoTask.Title = taskDto.Title;
            todoTask.IsCompleted = taskDto.IsCompleted;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ToDoTaskExists(id)) return NotFound();
                else throw;
            }

            return NoContent();
        }

        // DELETE: api/ToDoTasks/5 - Sletter en opgave
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteToDoTask(int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var todoTask = await _context.ToDoTasks
                .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

            if (todoTask == null) return NotFound();

            _context.ToDoTasks.Remove(todoTask);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ToDoTaskExists(int id)
        {
            return _context.ToDoTasks.Any(e => e.Id == id);
        }
    }
}