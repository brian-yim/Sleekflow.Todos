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
        string userId,
        IEnumerable<RequestFilterModel>? filters = null,
        RequestSortModel? sort = null
    )
    {
        var query = _context.Todos
            .Where(todo =>
                todo.CreatedBy == userId &&
                !todo.IsDeleted
            );

        foreach (var filter in filters ?? [])
        {
            FieldMapper.map.TryGetValue(filter.Field.ToLower(), out var filterField);
            if (!string.IsNullOrWhiteSpace(filterField))
            {
                query = query.Where(todo =>
                    EF.Property<object>(todo, filterField) == (object)filter.Value
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
            .Include(todo => todo.TodoTags)
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

    public async Task<ServiceResponseModel<Todo>> CreateAsync(
        string userId,
        TodoModel model
    )
    {
        var result = new ServiceResponseModel<Todo>();
        try
        {
            var todo = new Todo()
            {
                Name = model.Name,
                Description = model.Description,
                DueDate = model.DueDate,
                Status = nameof(Status.NotStarted),
                CreatedBy = userId,
                UpdatedBy = userId,
                TodoTags = model.Tags
                    .Select(tag => new TodoTag
                    {
                        Name = tag,
                    })
                    .ToList(),
            };

            await _context.Todos.AddAsync(todo);
            await _context.SaveChangesAsync();

            result.Data = todo;
        }
        catch (Exception ex)
        {
            result.Errors.Add(new ErrorModel(ex.Message));
        }

        return result;
    }

    public async Task<ServiceResponseModel<Todo>> UpdateAsync(
        string userId,
        Guid id,
        TodoModel model
    )
    {
        var responseModel = await GetAsync(id);

        if (responseModel.Data == null)
        {
            return responseModel;
        }

        if (responseModel.Data.CreatedBy != userId)
        {
            return new()
            {
                Errors = [new NotFoundError()]
            };
        }

        var todo = responseModel.Data;
        todo.Name = model.Name;
        todo.Description = model.Description;
        todo.DueDate = model.DueDate;
        todo.Priority = model.Priority;
        todo.UpdatedBy = userId;
        todo.UpdatedAt = DateTime.Now;

        if (todo.TodoTags?.Any() ?? false)
        {
            _context.TodoTags.RemoveRange(todo.TodoTags);
        }

        todo.TodoTags = model.Tags
            .Select(tag => new TodoTag
            {
                Name = tag,
            })
            .ToList();

        await _context.SaveChangesAsync();

        return responseModel;
    }

    public async Task<ServiceResponseModel> DeleteAsync(
        string userId,
        Guid id
    )
    {
        var responseModel = await GetAsync(id);

        if (responseModel.Data == null)
        {
            return new()
            {
                Errors = responseModel.Errors
            };
        }

        if (responseModel.Data.CreatedBy != userId)
        {
            return new()
            {
                Errors = [new NotFoundError()]
            };
        }
        
        responseModel.Data.IsDeleted = true;
        responseModel.Data.UpdatedBy = userId;
        responseModel.Data.UpdatedAt = DateTime.Now;

        await _context.SaveChangesAsync();

        return new();
    }
}