using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Data;
using TodoApi.Models;

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoItemsController : ControllerBase
    {
        private readonly ILogger<TodoItemsController> _logger;
        private readonly ITodoItemsRepository _repository;

        public TodoItemsController(ILogger<TodoItemsController> logger, ITodoItemsRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        // GET: TodoItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetTodoItems()
        {
            try
            {
                _logger.LogInformation("Request for TodoItems");
                List<TodoItem> todoItems = await _repository.GetTodoItems();
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
                TodoItem? todoItem = await _repository.GetTodoItem(id);
                if (todoItem == null)
                {
                    return NotFound();
                }
                _logger.LogInformation("Found TodoItem, id={}: {}", id, todoItem);
                return Ok(todoItem);
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
                TodoItem createdTodoItem = await _repository.CreateTodoItem(todoItem);
                _logger.LogInformation("Created TodoItem {}", createdTodoItem);
                return CreatedAtAction(
                    nameof(GetTodoItem),
                    new { id = createdTodoItem.Id },
                    createdTodoItem);
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
                TodoItem? todoItem = await _repository.SwitchTodoItem(id);
                if (todoItem == null)
                {
                    return NotFound();
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
                bool result = await _repository.DeleteTodoItem(id);
                if (!result)
                {
                    return NotFound();
                }
                _logger.LogInformation("Deleted TodoItem, id={}", id);
                return NoContent();
            }
            catch (Exception e)
            {
                return ErrorResult(e);
            }
        }

        private StatusCodeResult ErrorResult(Exception e)
        {
            _logger.LogError(e, "Error message: {}", e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}