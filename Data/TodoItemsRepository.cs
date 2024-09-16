using Microsoft.EntityFrameworkCore;
using TodoApi.Models;

namespace TodoApi.Data
{
    public class TodoItemsRepository : ITodoItemsRepository
    {
        private readonly TodoApiContext _context;
        private readonly ILogger<TodoItemsRepository> _logger;

        public TodoItemsRepository(TodoApiContext context, ILogger<TodoItemsRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<TodoItem>> GetTodoItems()
        {
            return await _context.TodoItems
                .OrderBy(item => item.Created)
                .ToListAsync();
        }

        public async Task<TodoItem?> GetTodoItem(long id)
        {
            TodoItem? todoItem = await _context.TodoItems.FindAsync(id);
            if (todoItem == null)
            {
                _logger.LogWarning("No TodoItem found, id={}", id);
                return null;
            }
            return todoItem;
        }

        public async Task<TodoItem> CreateTodoItem(TodoItem todoItem)
        {
            _context.TodoItems.Add(todoItem);
            await _context.SaveChangesAsync();
            return todoItem;
        }

        public async Task<TodoItem?> SwitchTodoItem(long id)
        {
            TodoItem? todoItem = await GetTodoItem(id);
            if (todoItem == null)
            {
                _logger.LogWarning("No TodoItem found on switching, id={}", id);
                return null;
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
                    return null;
                }
                else
                {
                    throw;
                }
            }

            return todoItem;
        }

        public async Task<bool> DeleteTodoItem(long id)
        {
            TodoItem? todoItem = await GetTodoItem(id);
            if (todoItem == null)
            {
                _logger.LogWarning("No TodoItem found on deleting, id={}", id);
                return false;
            }

            _context.TodoItems.Remove(todoItem);
            await _context.SaveChangesAsync();
            return true; // check saving result
        }

        private bool TodoItemExists(long id)
        {
            return _context.TodoItems.Any(e => e.Id == id);
        }
    }
}
