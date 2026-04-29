using devops.src.TodoApi.Models;

namespace devops.src.TodoApi.Services
{
    public class TodoService
    {
        private readonly List<TodoItem> _todos = new();
        private int _nextId = 1;

        public List<TodoItem> GetAll() => _todos.ToList();

        public TodoItem? GetById(int id) => _todos.FirstOrDefault(t => t.Id == id);

        public TodoItem Create(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Title cannot be empty.", nameof(title));

            var item = new TodoItem
            {
                Id = _nextId++,
                Title = title.Trim(),
                IsCompleted = false,
                CreatedAt = DateTime.UtcNow
            };
            _todos.Add(item);
            return item;
        }

        public TodoItem? Complete(int id)
        {
            var item = GetById(id);
            if (item is null) return null;
            item.IsCompleted = true;
            return item;
        }

        public bool Delete(int id)
        {
            var item = GetById(id);
            if (item is null) return false;
            _todos.Remove(item);
            return true;
        }
    }
}