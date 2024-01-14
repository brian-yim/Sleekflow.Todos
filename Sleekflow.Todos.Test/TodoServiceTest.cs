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
    private readonly ITodoService _todoService;
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
        _conetxt.Users.AddRange(MockUsers.GetList());

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
            .Where(todo => 
                !todo.IsDeleted &&
                todo.CreatedBy == "10b0f3fe-2893-4dfe-9448-ab37e8e86d95"
            )
            .Count();

        var result = await _todoService.GetAsync("10b0f3fe-2893-4dfe-9448-ab37e8e86d95");


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
            Value = "10b0f3fe-2893-4dfe-9448-ab37e8e86d94",
        };

        var recordCount = MockTodos.GetList()
            .Where(todo =>
                !todo.IsDeleted &&
                todo.Status == "Not Started" &&
                todo.CreatedBy == "10b0f3fe-2893-4dfe-9448-ab37e8e86d94"
            )
            .Count();

        var result = await _todoService.GetAsync("10b0f3fe-2893-4dfe-9448-ab37e8e86d94", [statusFilter, createdByFilter]);

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
                todo.Status == "Not Started" &&
                todo.CreatedBy == "10b0f3fe-2893-4dfe-9448-ab37e8e86d95"
            )
            .OrderByDescending(todo => todo.CreatedAt);

        var result = await _todoService.GetAsync("10b0f3fe-2893-4dfe-9448-ab37e8e86d95", [statusFilter], sort);

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
    public async void GetByIdAsync_WithTags_Success()
    {
        var testId = new Guid("7984908b-3f91-4a6c-a651-85119f31eda2");
        var result = await _todoService.GetAsync(testId);

        var mockRecord = MockTodos.GetList()
                    .Where(todo => todo.Id == testId)
                    .FirstOrDefault();

        Assert.NotNull(result.Data);
        Assert.Empty(result.Errors);

        Assert.NotNull(result.Data.TodoTags);
        Assert.Equal(mockRecord?.TodoTags?.Count(), result.Data.TodoTags.Count());
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

        var result = await _todoService.CreateAsync("10b0f3fe-2893-4dfe-9448-ab37e8e86d95", todoModel);

        Assert.NotNull(result.Data);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public async void CreateAsync_WithTag_Success()
    {
        var todoModel = new TodoModel()
        {
            Name = "Test Create",
            Description = "Test Description",
            DueDate = new DateTime(2025, 1, 1),
            Tags = ["test", "test2"],
        };

        var result = await _todoService.CreateAsync("10b0f3fe-2893-4dfe-9448-ab37e8e86d95", todoModel);

        Assert.NotNull(result.Data);
        Assert.Empty(result.Errors);

        Assert.NotNull(result.Data.TodoTags);
        Assert.Equal(todoModel.Tags.Count(), result.Data.TodoTags.Count());
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

        var result = await _todoService.UpdateAsync("10b0f3fe-2893-4dfe-9448-ab37e8e86d95", testId, todoModel);

        Assert.NotNull(result.Data);
        Assert.Empty(result.Errors);

        Assert.Equal(todoModel.Name, result.Data.Name);
        Assert.Equal(todoModel.Description, result.Data.Description);
        Assert.Equal(todoModel.DueDate, result.Data.DueDate);
    }

    [Fact]
    public async void UpdateAsync_WithTags_Success()
    {
        var testId = new Guid("7984908b-3f91-4a6c-a671-85119f41eda7");

        var todoModel = new TodoModel()
        {
            Name = "Test Update Name",
            Description = "Test Update Description",
            DueDate = new DateTime(2025, 1, 1),
            Tags = ["test1", "test2"]
        };

        var result = await _todoService.UpdateAsync("10b0f3fe-2893-4dfe-9448-ab37e8e86d95", testId, todoModel);

        Assert.NotNull(result.Data);
        Assert.Empty(result.Errors);

        Assert.Equal(todoModel.Name, result.Data.Name);
        Assert.Equal(todoModel.Description, result.Data.Description);
        Assert.Equal(todoModel.DueDate, result.Data.DueDate);

        Assert.NotNull(result.Data.TodoTags);
        Assert.Equal(todoModel.Tags.Count(), result.Data.TodoTags.Count());
    }

    [Fact]
    public async void UpdateAsync_WithDeleteTags_Success()
    {
        var testId = new Guid("7984908b-3f91-4a6c-a671-85119f41eda7");

        var todoModel = new TodoModel()
        {
            Name = "Test Update Name",
            Description = "Test Update Description",
            DueDate = new DateTime(2025, 1, 1),
            Tags = ["test1"]
        };

        var result = await _todoService.UpdateAsync("10b0f3fe-2893-4dfe-9448-ab37e8e86d95", testId, todoModel);

        Assert.NotNull(result.Data);
        Assert.Empty(result.Errors);

        Assert.Equal(todoModel.Name, result.Data.Name);
        Assert.Equal(todoModel.Description, result.Data.Description);
        Assert.Equal(todoModel.DueDate, result.Data.DueDate);

        Assert.NotNull(result.Data.TodoTags);
        Assert.Equal(todoModel.Tags.Count(), result.Data.TodoTags.Count());
    }


    [Fact]
    public async void UpdateAsync_NotOwn_Fail()
    {
        var testId = new Guid("7984908b-3f91-4a6c-a671-85119f41eda7");

        var todoModel = new TodoModel()
        {
            Name = "Test Update Name",
            Description = "Test Update Description",
            DueDate = new DateTime(2025, 1, 1),
            Tags = ["test1"]
        };

        var result = await _todoService.UpdateAsync("10b0f3fe-2893-4dfe-9448-ab37e8e86d94", testId, todoModel);

        Assert.Null(result.Data);
        Assert.NotEmpty(result.Errors);
        Assert.IsType<NotFoundError>(result.Errors.First());
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

        var result = await _todoService.UpdateAsync("10b0f3fe-2893-4dfe-9448-ab37e8e86d95", testId, todoModel);

        Assert.Null(result.Data);
        Assert.NotEmpty(result.Errors);
        Assert.IsType<NotFoundError>(result.Errors.First());
    }

    [Fact]
    public async void DeleteAsync_Success()
    {
        var testId = new Guid("7984908b-3f91-4a6c-a671-85119f41eda7");

        var result = await _todoService.DeleteAsync("10b0f3fe-2893-4dfe-9448-ab37e8e86d95", testId);

        Assert.Empty(result.Errors);
    }

    [Fact]
    public async void DeleteAsync_NotOwn_Fail()
    {
        var testId = new Guid("7984908b-3f91-4a6c-a671-85119f41eda7");

        var result = await _todoService.DeleteAsync("10b0f3fe-2893-4dfe-9448-ab37e8e86d94", testId);

        Assert.NotEmpty(result.Errors);
        Assert.IsType<NotFoundError>(result.Errors.First());
    }

    [Fact]
    public async void DeleteAsync_Deleted_NotFound_Fail()
    {
        var testId = new Guid("7984908b-3f91-4a6c-a651-85119f41eda8");

        var result = await _todoService.DeleteAsync("10b0f3fe-2893-4dfe-9448-ab37e8e86d95", testId);

        Assert.NotEmpty(result.Errors);
        Assert.IsType<NotFoundError>(result.Errors.First());
    }
}