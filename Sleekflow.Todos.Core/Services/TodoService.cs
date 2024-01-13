using Microsoft.EntityFrameworkCore;
using Sleekflow.Todos.Core.Enums;
using Sleekflow.Todos.Core.Helpers;
using Sleekflow.Todos.Core.Models;
using Sleekflow.Todos.DAL;
using Sleekflow.Todos.DAL.Models;

namespace Sleekflow.Todos.Core.Services;

public class TodoService(TodoContext context) : ITodoService
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
            FieldMapper.map.TryGetValue(filter.Field.ToLower(), out var filterField);
            if (!string.IsNullOrWhiteSpace(filterField))
            {
                query = query.Where(todo =>
                    EF.Property<object>(todo, filterField) == filter.Value
                );

            }
        }

        if (sort != null)
        {
            FieldMapper.map.TryGetValue(sort.Field.ToLower(), out var sortedField);
            if (!string.IsNullOrWhiteSpace(sortedField))
            {
                query = sort.Direction == "+" ?
                    query.OrderBy(todo => EF.Property<object>(todo, sortedField)) :
                    query.OrderByDescending(todo => EF.Property<object>(todo, sortedField));
            }
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