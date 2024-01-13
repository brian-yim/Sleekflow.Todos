using System.Linq;
using Microsoft.EntityFrameworkCore;
using Sleekflow.Todos.Core.Models;
using Sleekflow.Todos.DAL;
using Sleekflow.Todos.DAL.Models;

namespace Sleekflow.Todos.Core.Services;

public class TodoService(TodoContext context) : ITodoServie
{
    private readonly TodoContext _context = context;

    public async Task GetAsync()
    {

    }

    public async Task<ServiceResponseModel<Todo>> GetAsync(Guid id)
    {
        var result = new ServiceResponseModel<Todo>();

        var todo = await _context.Todos
            .Where(todo => todo.Id == id)
            .FirstOrDefaultAsync();

        if (todo == null)
        {
            result.Errors.Add(new NotFoundError());
            return result;
        }

        result.Data = todo;
        return result;
    }

    public async Task CreateAsync(TodoModel todo)
    {

    }

    public async Task UpdateAsync(Guid id, TodoModel todo)
    {

    }

    public async Task DeleteAsync(Guid id)
    {

    }
}