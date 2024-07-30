using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Data;
using TodoApi.Models;

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoItemsController(ILogger<TodoItemsController> logger, TodoApiContext context) : ControllerBase
    {
        private readonly ILogger<TodoItemsController> _logger = logger;
        private readonly TodoApiContext _context = context;

        // GET: TodoItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetTodoItems()
        {
            try
            {
                _logger.LogInformation("Request for TodoItems");
                List<TodoItem> todoItems = await _context.TodoItems
                    .OrderBy(item => item.Created)
                    .ToListAsync();
                _logger.LogInformation("Found TodoItems: {}", todoItems);

                return Ok(todoItems);
            }
            catch (Exception e)
            {
                return ErrorResult(e);
            }
        }

        // GET: TodoItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItem>> GetTodoItem(long id)
        {
            try
            {
                _logger.LogInformation("Request for TodoItem, id={}", id);
                var todoItem = await _context.TodoItems.FindAsync(id);

                if (todoItem == null)
                {
                    _logger.LogWarning("No TodoItem found, id={}", id);
                    return NotFound();
                }

                _logger.LogInformation("Found TodoItem, id={}: {}", id, todoItem);
                return todoItem;
            }
            catch (Exception e)
            {
                return ErrorResult(e);
            }
        }

        // POST: TodoItems
        [HttpPost]
        public async Task<ActionResult<TodoItem>> CreateTodoItem(TodoItem todoItem)
        {
            try
            {
                _logger.LogInformation("Request to create TodoItem {}", todoItem);
                _context.TodoItems.Add(todoItem);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Created TodoItem {}", todoItem);

                return CreatedAtAction(
                    nameof(GetTodoItem),
                    new { id = todoItem.Id },
                    todoItem);
            }
            catch (Exception e)
            {
                return ErrorResult(e);
            }
        }

        // PATCH: TodoItems/5
        [HttpPatch("{id}")]
        public async Task<IActionResult> SwitchTodoItem(long id)
        {
            try
            {
                _logger.LogInformation("Request to switch TodoItem, id={}", id);
                var todoItem = await _context.TodoItems.FindAsync(id);
                if (todoItem == null)
                {
                    _logger.LogWarning("No TodoItem found, id={}", id);
                    return NotFound();
                }

                todoItem.IsComplete = !todoItem.IsComplete;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException) when (!TodoItemExists(id))
                {
                    if (!TodoItemExists(id))
                    {
                        _logger.LogWarning("No TodoItem found on saving, id={}", id);
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                _logger.LogInformation("Switched TodoItem to {}, id={}", todoItem.IsComplete, id);
                return NoContent();
            }
            catch (Exception e)
            {
                return ErrorResult(e);
            }
        }

        // DELETE: TodoItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoItem(long id)
        {
            try
            {
                _logger.LogInformation("Request to delete TodoItem, id={}", id);
                var todoItem = await _context.TodoItems.FindAsync(id);
                if (todoItem == null)
                {
                    _logger.LogWarning("No TodoItem found, id={}", id);
                    return NotFound();
                }

                _context.TodoItems.Remove(todoItem);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Deleted TodoItem, id={}", id);
                return NoContent();
            }
            catch (Exception e)
            {
                return ErrorResult(e);
            }
        }

        private bool TodoItemExists(long id)
        {
            return _context.TodoItems.Any(e => e.Id == id);
        }

        private StatusCodeResult ErrorResult(Exception e)
        {
            _logger.LogError(e, "Error message: {}", e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}