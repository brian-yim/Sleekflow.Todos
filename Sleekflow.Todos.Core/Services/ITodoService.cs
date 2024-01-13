using Sleekflow.Todos.Core.Models;

namespace Sleekflow.Todos.Core.Services;

public interface ITodoServie {
    Task GetAsync(Guid id);
    Task CreateAsync(TodoModel todo);
    Task UpdateAsync(Guid id, TodoModel todo);
    Task DeleteAsync(Guid id);
}