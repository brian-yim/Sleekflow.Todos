using Sleekflow.Todos.Core.Services;
using Sleekflow.Todos.Test.Mocks;

namespace Sleekflow.Todos.Test;

public class TodoServiceTest : IDisposable
{
    private readonly ITodoServie _todoService;
    public TodoServiceTest()
    {
        _todoService = new TodoService();
    }

    public void Dispose()
    {
        // throw new NotImplementedException();
    }

    [Fact]
    public async void CreateAsync_Success()
    {
        await _todoService.CreateAsync(MockTodoModel.Create());
    }
}