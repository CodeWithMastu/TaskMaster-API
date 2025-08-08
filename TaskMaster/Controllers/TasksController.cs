using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskMaster.DTOs;
using TaskMaster.Models;
using TaskMaster.Services;

namespace TaskMaster.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TasksController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TasksController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/tasks
        [HttpGet]
        public IActionResult GetAllTasks()
        {
            var userId = GetUserId();
            if (userId == null) return Unauthorized();

            var tasks = _context.Tasks
                .Where(t => t.UserId == userId)
                .ToList();

            return Ok(tasks);
        }

        // GET: api/tasks/5
        [HttpGet("{id}")]
        public IActionResult GetTask(int id)
        {
            var userId = GetUserId();
            if (userId == null) return Unauthorized();

            var task = _context.Tasks.FirstOrDefault(t => t.Id == id && t.UserId == userId);
            if (task == null) return NotFound();

            return Ok(task);
        }

        // POST: api/tasks
        [HttpPost]
        public IActionResult CreateTask(TaskDto taskDto)
        {
            var userId = GetUserId();
            if (userId == null) return Unauthorized();

            var task = new TasksItem
            {
                Title = taskDto.Title,
                Description = taskDto.Description,
                Deadline = taskDto.Deadline,
                Priority = taskDto.Priority,
                IsCompleted = false,
                UserId = userId.Value
            };

            _context.Tasks.Add(task);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetTask), new { id = task.Id }, task);
        }

        // PUT: api/tasks/5
        [HttpPut("{id}")]
        public IActionResult UpdateTask(int id, TaskDto taskDto)
        {
            var userId = GetUserId();
            if (userId == null) return Unauthorized();

            var task = _context.Tasks.FirstOrDefault(t => t.Id == id && t.UserId == userId);
            if (task == null) return NotFound();

            task.Title = taskDto.Title;
            task.Description = taskDto.Description;
            task.Deadline = taskDto.Deadline;
            task.Priority = taskDto.Priority;

            _context.SaveChanges();
            return NoContent();
        }

        // DELETE: api/tasks/5
        [HttpDelete("{id}")]
        public IActionResult DeleteTask(int id)
        {
            var userId = GetUserId();
            if (userId == null) return Unauthorized();

            var task = _context.Tasks.FirstOrDefault(t => t.Id == id && t.UserId == userId);
            if (task == null) return NotFound();

            _context.Tasks.Remove(task);
            _context.SaveChanges();

            return NoContent();
        }

        // Helper method to get user ID from JWT
        private int? GetUserId()
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (int.TryParse(userIdString, out var userId))
            {
                return userId;
            }
            return null;
        }
    }
}
