using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoListApp.Models;

namespace ToDoListApp.Controllers
{
    // Definerer routingmønsteret for controlleren
    [Route("api/[controller]")]
    [ApiController]
    public class ToDoTasksController : ControllerBase
    {
        // Dependency Injection af AppDbContext
        private readonly AppDbContext _context;

        public ToDoTasksController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/ToDoTasks - Henter alle opgaver
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ToDoTask>>> GetToDoTasks()
        {
            return await _context.ToDoTasks.ToListAsync();
        }

        // GET: api/ToDoTasks/8 - Henter en specifik opgave baseret på id
        [HttpGet("{id}")]
        public async Task<ActionResult<ToDoTask>> GetToDoTask(int id)
        {
            var todoTask = await _context.ToDoTasks.FindAsync(id);

            if (todoTask == null)
            {
                return NotFound();
            }

            return todoTask;
        }

        // POST: api/ToDoTasks - Opretter en ny opgave
        [HttpPost]
        public async Task<ActionResult<ToDoTask>> PostToDoTask(ToDoTask todoTask)
        {
            _context.ToDoTasks.Add(todoTask);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetToDoTask", new { id = todoTask.Id }, todoTask);
        }

        // PUT: api/ToDoTasks/5 - Opdaterer en eksisterende opgave
        [HttpPut("{id}")]
        public async Task<IActionResult> PutToDoTask(int id, ToDoTask todoTask)
        {
            if (id != todoTask.Id)
            {
                return BadRequest();
            }

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

        // DELETE: api/ToDoTasks/5 - Sletter en opgave
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteToDoTask(int id)
        {
            var todoTask = await _context.ToDoTasks.FindAsync(id);
            if (todoTask == null)
            {
                return NotFound();
            }

            _context.ToDoTasks.Remove(todoTask);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Hjælpefunktion til at kontrollere, om en opgave eksisterer
        private bool ToDoTaskExists(int id)
        {
            return _context.ToDoTasks.Any(e => e.Id == id);
        }
    }
}