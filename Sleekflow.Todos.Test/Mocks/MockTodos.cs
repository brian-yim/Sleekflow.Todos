using Sleekflow.Todos.DAL.Models;

public static class MockTodos
{
    public static List<Todo> GetList()
    {
        return new() {
            new() {
                Id = new Guid("7984908b-3f91-4a6c-a671-85119f41eda7"),
                Name = "Test Name 1",
                Description = "Test Description 1",
                Status = "Not Started",
                CreatedBy = "Mock",
                UpdatedBy = "Mock",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            },
            new() {
                Id = new Guid("7984908b-3f91-4a6c-a671-85119f41eda8"),
                Name = "Test Name 2",
                Description = "Test Description 2",
                Status = "Not Started",
                CreatedBy = "Mock",
                UpdatedBy = "Mock",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            }
        };
    }
}