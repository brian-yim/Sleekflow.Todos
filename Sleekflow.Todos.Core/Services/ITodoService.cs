using Sleekflow.Todos.Core.Models;
using Sleekflow.Todos.DAL.Models;

namespace Sleekflow.Todos.Core.Services;

public interface ITodoServie
{
    Task GetAsync();
    Task<ServiceResponseModel<Todo>> GetAsync(Guid id);
    Task CreateAsync(TodoModel todo);
    Task UpdateAsync(Guid id, TodoModel todo);
    Task DeleteAsync(Guid id);
}