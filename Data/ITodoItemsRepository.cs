using TodoApi.Models;

namespace TodoApi.Data
{
    public interface ITodoItemsRepository
    {
        Task<List<TodoItem>> GetTodoItems();
        Task<TodoItem?> GetTodoItem(long id);
        Task<TodoItem> CreateTodoItem(TodoItem todoItem);
        Task<TodoItem?> SwitchTodoItem(long id);
        Task<bool> DeleteTodoItem(long id);
    }
}
