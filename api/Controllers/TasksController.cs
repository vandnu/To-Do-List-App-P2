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

        // GET: api/ToDoTasks/my-tasks
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
                    UserId = t.UserId,
                    Deadline = t.Deadline
                })
                .ToListAsync();
            return Ok(tasks);
        }

        // GET: api/ToDoTasks/all-tasks
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
                    UserId = t.UserId,
                    Deadline = t.Deadline
                })
                .ToListAsync();
            return Ok(tasks);
        }

        // GET: api/ToDoTasks/{id}
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<TaskDto>> GetToDoTask(int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

            ToDoTask? todoTask;

            if (userRole == "Admin")
            {
                todoTask = await _context.ToDoTasks.FirstOrDefaultAsync(t => t.Id == id);
            }
            else
            {
                todoTask = await _context.ToDoTasks.FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);
            }

            if (todoTask == null)
            {
                return NotFound("Task not found or you don't have permission to access this task.");
            }

            var taskDto = new TaskDto
            {
                Id = todoTask.Id,
                Title = todoTask.Title,
                IsCompleted = todoTask.IsCompleted,
                Description = todoTask.Description,
                UserId = todoTask.UserId,
                Deadline = todoTask.Deadline
            };
            return Ok(taskDto);
        }

        // POST: api/ToDoTasks
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<TaskDto>> PostToDoTask(TaskDto taskDto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            
            // Opretter ToDoTask (model) fra TaskDto
            var todoTask = new ToDoTask
            {
                Title = taskDto.Title,
                Description = taskDto.Description,
                IsCompleted = taskDto.IsCompleted,
                UserId = userId,
                Deadline = taskDto.Deadline

            };

            _context.ToDoTasks.Add(todoTask);
            await _context.SaveChangesAsync();

            var createdTaskDto = new TaskDto
            {
                Id = todoTask.Id,
                Title = todoTask.Title,
                Description = todoTask.Description,
                IsCompleted = todoTask.IsCompleted,
                UserId = todoTask.UserId,
                Deadline = todoTask.Deadline

            };
            return CreatedAtAction(nameof(GetToDoTask), new { id = todoTask.Id }, createdTaskDto);
        }

        // PUT: api/ToDoTasks/{id}
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateTask(int id, [FromBody] TaskDto updatedTaskDto) // Modtager nu TaskDto
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

            ToDoTask? todoTask = userRole == "Admin"
                ? await _context.ToDoTasks.FirstOrDefaultAsync(t => t.Id == id)
                : await _context.ToDoTasks.FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

            if (todoTask == null)
            {
                return NotFound("Task not found or you don't have permission to update it.");
            }


            if (id != updatedTaskDto.Id)
            {
                 return BadRequest("ID mismatch between URL and task data.");
            }

            // Opdatér felterne på den fundne database-entitet
            todoTask.Title = updatedTaskDto.Title;
            todoTask.Description = updatedTaskDto.Description;
            todoTask.IsCompleted = updatedTaskDto.IsCompleted;
            todoTask.Deadline = updatedTaskDto.Deadline;

            _context.Entry(todoTask).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ToDoTaskExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }

        // PATCH: api/ToDoTasks/{id}/complete
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
            {
                return NotFound("Task not found or you don't have permission to update it.");
            }

            todoTask.IsCompleted = isCompleted;
            await _context.SaveChangesAsync();
            
            // Returner den opdaterede task som DTO
            var updatedDto = new TaskDto {
                Id = todoTask.Id,
                Title = todoTask.Title,
                Description = todoTask.Description,
                IsCompleted = todoTask.IsCompleted,
                Deadline = todoTask.Deadline,
                UserId = todoTask.UserId
                // ... andre felter ...
            };
            return Ok(updatedDto);
        }

        // DELETE: api/ToDoTasks/{id}
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
            ToDoTask? todoTask;

            if (userRole == "Admin")
            {
                todoTask = await _context.ToDoTasks.FirstOrDefaultAsync(t => t.Id == id);
            }
            else
            {
                todoTask = await _context.ToDoTasks.FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);
            }

            if (todoTask == null)
            {
                return NotFound("Task not found or you don't have permission to delete it.");
            }

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