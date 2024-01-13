using Sleekflow.Todos.Core.Models;

namespace Sleekflow.Todos.Test.Mocks;
public static class MockTodoModel
{
    public static TodoModel Create()
    {
        return new()
        {
            Name = "Test Name",
            Description = "Test Descriptiion"
        };
    }
}