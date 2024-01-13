using Microsoft.EntityFrameworkCore;
using Sleekflow.Todos.Core.Enums;
using Sleekflow.Todos.Core.Models;
using Sleekflow.Todos.DAL;
using Sleekflow.Todos.DAL.Models;

namespace Sleekflow.Todos.Core.Services;

public class TodoService(TodoContext context) : ITodoServie
{
    private readonly TodoContext _context = context;

    public async Task<ServiceResponseModel<List<Todo>>> GetAsync(
        IEnumerable<RequestFilterModel>? filters = null,
        RequestSortModel? sort = null
    )
    {
        var query = _context.Todos.Where(todo => !todo.IsDeleted);

        foreach (var filter in filters ?? [])
        {
            query = query.Where(q =>
                EF.Property<object>(q, filter.Field) == filter.Value
            );
        }

        if (sort != null)
        {
            query = sort.Direction == "+" ?
                query.OrderBy(todo => EF.Property<object>(todo, sort.Field)) :
                query.OrderByDescending(todo => EF.Property<object>(todo, sort.Field))
            ;
        }

        return new()
        {
            Data = await query.ToListAsync()
        };
    }

    public async Task<ServiceResponseModel<Todo>> GetAsync(Guid id)
    {
        var result = new ServiceResponseModel<Todo>();

        var todo = await _context.Todos
            .Where(todo =>
                !todo.IsDeleted &&
                todo.Id == id
            )
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

    public async Task<ServiceResponseModel<Todo>> UpdateAsync(Guid id, TodoModel model)
    {
        var responseModel = await GetAsync(id);

        if (responseModel.Data == null)
        {
            return responseModel;
        }

        var todo = responseModel.Data;
        todo.Name = model.Name;
        todo.Description = model.Description;
        todo.DueDate = model.DueDate;

        await _context.SaveChangesAsync();

        return responseModel;
    }

    public async Task<ServiceResponseModel> DeleteAsync(Guid id)
    {
        var responseModel = await GetAsync(id);

        if (responseModel.Data == null)
        {
            return new()
            {
                Errors = responseModel.Errors
            };
        }

        responseModel.Data.IsDeleted = true;

        await _context.SaveChangesAsync();

        return new();
    }
}