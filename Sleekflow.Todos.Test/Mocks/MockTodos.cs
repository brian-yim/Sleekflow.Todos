using Sleekflow.Todos.DAL.Models;

namespace Sleekflow.Todos.Test.Mocks;

public static class MockTodos
{
    public static List<Todo> GetList()
    {
        return [
            new()
            {
                Id = new Guid("7984908b-3f91-4a6c-a671-85119f41eda7"),
                Name = "Test Name 1",
                Description = "Test Description 1",
                Status = "Not Started",
                CreatedBy = "Mock",
                UpdatedBy = "Mock",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            },
            new()
            {
                Id = new Guid("7984908b-3f91-4a6c-a671-85119f41eda8"),
                Name = "Test Name 2",
                Description = "Test Description 2",
                Status = "Completed",
                CreatedBy = "Mock",
                UpdatedBy = "Mock",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            },
            new()
            {
                Id = new Guid("7984908b-3f91-4a6c-a651-85119f41eda8"),
                Name = "Test Name 3",
                Description = "Test Description 3",
                Status = "Not Started",
                CreatedBy = "Mock",
                UpdatedBy = "Mock",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                IsDeleted = true,
            },
            new()
            {
                Id = new Guid("7984908b-3f91-4a6c-a651-85119f31eda8"),
                Name = "Test Name 4",
                Description = "Test Description 4",
                Status = "Not Started",
                CreatedBy = "Mock1",
                UpdatedBy = "Mock1",
                CreatedAt = DateTime.Now.AddDays(1),
                UpdatedAt = DateTime.Now.AddDays(1),
            }
        ];
    }
}