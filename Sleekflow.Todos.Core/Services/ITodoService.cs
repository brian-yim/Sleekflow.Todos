using Sleekflow.Todos.Core.Models;
using Sleekflow.Todos.DAL.Models;

namespace Sleekflow.Todos.Core.Services;

public interface ITodoService
{
    Task<ServiceResponseModel<List<Todo>>> GetAsync(
        IEnumerable<RequestFilterModel>? filters = null,
        RequestSortModel? sort = null
    );
    Task<ServiceResponseModel<Todo>> GetAsync(Guid id);
    Task<ServiceResponseModel<Todo>> CreateAsync(TodoModel model);
    Task<ServiceResponseModel<Todo>> UpdateAsync(Guid id, TodoModel model);
    Task<ServiceResponseModel> DeleteAsync(Guid id);
}