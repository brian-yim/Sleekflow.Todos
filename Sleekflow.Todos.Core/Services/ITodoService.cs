using Sleekflow.Todos.Core.Models;
using Sleekflow.Todos.DAL.Models;

namespace Sleekflow.Todos.Core.Services;

public interface ITodoServie
{
    Task<ServiceResponseModel<List<Todo>>> GetAsync();
    Task<ServiceResponseModel<Todo>> GetAsync(Guid id);
    Task<ServiceResponseModel<Todo>> CreateAsync(TodoModel model);
    Task<ServiceResponseModel<Todo>> UpdateAsync(Guid id, TodoModel model);
    Task DeleteAsync(Guid id);
}