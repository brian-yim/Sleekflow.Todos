using Microsoft.EntityFrameworkCore;
using Sleekflow.Todos.Core.Enums;
using Sleekflow.Todos.Core.Models;
using Sleekflow.Todos.DAL;
using Sleekflow.Todos.DAL.Models;

namespace Sleekflow.Todos.Core.Services;

public class TodoService(TodoContext context) : ITodoServie
{
    private readonly TodoContext _context = context;

    public async Task<ServiceResponseModel<List<Todo>>> GetAsync()
    {
        return new()
        {
            Data = await _context.Todos.ToListAsync()
        };
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

    public async Task<ServiceResponseModel<Todo>> CreateAsync(TodoModel model)
    {
        var result = new ServiceResponseModel<Todo>();
        try
        {
            //TODO: Get username
            var todo = new Todo()
            {
                Name = model.Name,
                Description = model.Description,
                DueDate = model.DueDate,
                Status = nameof(Status.NotStarted),
                CreatedBy = "",
                UpdatedBy = ""
            };

            await _context.Todos.AddAsync(todo);
            await _context.SaveChangesAsync();

            result.Data = todo;
        }
        catch (Exception ex)
        {
            //TODO: add log
            result.Errors.Add(new ErrorModel(ex.Message));
        }

        return result;
    }

    public async Task UpdateAsync(Guid id, TodoModel model)
    {

    }

    public async Task DeleteAsync(Guid id)
    {

    }
}