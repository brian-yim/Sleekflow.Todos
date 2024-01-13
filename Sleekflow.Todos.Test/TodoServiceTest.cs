using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
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
    public async void CreateAsync_Success()
    {
        await _todoService.CreateAsync(MockTodoModel.Create());
    }

    [Fact]
    public async void GetAsync_Success()
    {
        var testId = new Guid("7984908b-3f91-4a6c-a671-85119f41eda7");
        var result = await _todoService.GetAsync(testId);

        Assert.NotNull(result.Data);
        Assert.Empty(result.Errors);
    }
}