using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Sleekflow.Todos.Core.Models;
using Sleekflow.Todos.Core.Services;
using Sleekflow.Todos.DAL;
using Sleekflow.Todos.Test.Mocks;

namespace Sleekflow.Todos.Test;

public class TodoServiceTest : IDisposable
{
    private readonly TodoContext _conetxt;
    private readonly ITodoServie _todoService;
    private readonly SqliteConnection _connection;

    public TodoServiceTest()
    {
        _connection = new SqliteConnection("Filename=:memory:");
        _connection.Open();

        var options = new DbContextOptionsBuilder<TodoContext>()
            .UseSqlite(_connection)
            .Options;

        _conetxt = new TodoContext(options);
        _conetxt.Database.EnsureDeleted();
        _conetxt.Database.EnsureCreated();

        _conetxt.Todos.AddRange(MockTodos.GetList());

        _conetxt.SaveChanges();

        _todoService = new TodoService(_conetxt);
    }

    public void Dispose()
    {
        _connection.Dispose();
    }

    [Fact]
    public async void GetAsync_Success()
    {
        var recordCount = MockTodos.GetList()
            .Where(todo => !todo.IsDeleted)
            .Count();

        var result = await _todoService.GetAsync();


        Assert.NotNull(result.Data);

        Assert.Equal(recordCount, result.Data.Count);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public async void GetAsync_WithFilters_Success()
    {
        var statusFilter = new RequestFilterModel()
        {
            Field = "Status",
            Value = "Not Started"
        };
        var createdByFilter = new RequestFilterModel()
        {
            Field = "CreatedBy",
            Value = "Mock1"
        };

        var recordCount = MockTodos.GetList()
            .Where(todo =>
                !todo.IsDeleted &&
                todo.Status == "Not Started" &&
                todo.CreatedBy == "Mock1"
            )
            .Count();

        var result = await _todoService.GetAsync([statusFilter, createdByFilter]);

        Assert.NotNull(result.Data);

        Assert.Equal(recordCount, result.Data.Count);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public async void GetAsync_WithSort_Success()
    {
        var statusFilter = new RequestFilterModel()
        {
            Field = "Status",
            Value = "Not Started"
        };

        var sort = new RequestSortModel()
        {
            Field = "CreatedAt",
            Direction = "-"
        };

        var records = MockTodos.GetList()
            .Where(todo =>
                !todo.IsDeleted &&
                todo.Status == "Not Started"
            )
            .OrderByDescending(todo => todo.CreatedAt);

        var result = await _todoService.GetAsync([statusFilter], sort);

        Assert.NotNull(result.Data);

        Assert.Equal(records.Count(), result.Data.Count);
        Assert.Empty(result.Errors);

        Assert.Equal(
            records.Select(todo => todo.Id).FirstOrDefault(),
            result.Data.Select(todo => todo.Id).FirstOrDefault()
        );
    }

    [Fact]
    public async void GetByIdAsync_Success()
    {
        var testId = new Guid("7984908b-3f91-4a6c-a671-85119f41eda7");
        var result = await _todoService.GetAsync(testId);

        Assert.NotNull(result.Data);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public async void GetByIdAsync_NotFound_Fail()
    {
        var testId = new Guid("7984908b-3f91-4a2c-a671-85119f41eda7");
        var result = await _todoService.GetAsync(testId);

        Assert.Null(result.Data);
        Assert.NotEmpty(result.Errors);
        Assert.IsType<NotFoundError>(result.Errors.First());
    }

    [Fact]
    public async void GetByIdAsync_Deleted_NotFound_Fail()
    {
        var testId = new Guid("7984908b-3f91-4a6c-a651-85119f41eda8");
        var result = await _todoService.GetAsync(testId);

        Assert.Null(result.Data);
        Assert.NotEmpty(result.Errors);
        Assert.IsType<NotFoundError>(result.Errors.First());
    }

    [Fact]
    public async void CreateAsync_Success()
    {
        var todoModel = new TodoModel()
        {
            Name = "Test Create",
            Description = "Test Description",
            DueDate = new DateTime(2025, 1, 1),
        };

        var result = await _todoService.CreateAsync(todoModel);

        Assert.NotNull(result.Data);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public async void UpdateAsync_Success()
    {
        var testId = new Guid("7984908b-3f91-4a6c-a671-85119f41eda7");

        var todoModel = new TodoModel()
        {
            Name = "Test Update Name",
            Description = "Test Update Description",
            DueDate = new DateTime(2025, 1, 1),
        };

        var result = await _todoService.UpdateAsync(testId, todoModel);

        Assert.NotNull(result.Data);
        Assert.Empty(result.Errors);

        Assert.Equal(todoModel.Name, result.Data.Name);
        Assert.Equal(todoModel.Description, result.Data.Description);
        Assert.Equal(todoModel.DueDate, result.Data.DueDate);
    }

    [Fact]
    public async void UpdateAsync_Deleted_NotFound_Fail()
    {
        var testId = new Guid("7984908b-3f91-4a6c-a651-85119f41eda8");

        var todoModel = new TodoModel()
        {
            Name = "Test Update Name",
            Description = "Test Update Description",
            DueDate = new DateTime(2025, 1, 1),
        };

        var result = await _todoService.UpdateAsync(testId, todoModel);

        Assert.Null(result.Data);
        Assert.NotEmpty(result.Errors);
        Assert.IsType<NotFoundError>(result.Errors.First());
    }

    [Fact]
    public async void DeleteAsync_Success()
    {
        var testId = new Guid("7984908b-3f91-4a6c-a671-85119f41eda7");

        var result = await _todoService.DeleteAsync(testId);

        Assert.Empty(result.Errors);
    }

    [Fact]
    public async void DeleteAsync_Deleted_NotFound_Fail()
    {
        var testId = new Guid("7984908b-3f91-4a6c-a651-85119f41eda8");

        var result = await _todoService.DeleteAsync(testId);

        Assert.NotEmpty(result.Errors);
        Assert.IsType<NotFoundError>(result.Errors.First());
    }
}