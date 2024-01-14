using Sleekflow.Todos.Core.Models;
using Sleekflow.Todos.DAL.Models;

namespace Sleekflow.Todos.Core.Services;

public interface ITodoService
{
    Task<ServiceResponseModel<List<Todo>>> GetAsync(
        string userId,
        IEnumerable<RequestFilterModel>? filters = null,
        RequestSortModel? sort = null
    );
    
    Task<ServiceResponseModel<Todo>> GetAsync(Guid id);

    Task<ServiceResponseModel<Todo>> CreateAsync(
        string userId, 
        TodoModel model
    );

    Task<ServiceResponseModel<Todo>> UpdateAsync(
        string userId, 
        Guid id, 
        TodoModel model
    );

    Task<ServiceResponseModel> DeleteAsync(
        string userId, 
        Guid id
    );
}