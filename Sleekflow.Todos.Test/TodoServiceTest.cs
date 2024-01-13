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
        var recordCount = MockTodos.GetList().Count;
        var result = await _todoService.GetAsync();

        Assert.NotNull(result.Data);

        Assert.Equal(recordCount, result.Data.Count);
        Assert.Empty(result.Errors);
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
    public async void GetByIdAsync_NotFound_Success()
    {
        var testId = new Guid("7984908b-3f91-4a2c-a671-85119f41eda7");
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
}