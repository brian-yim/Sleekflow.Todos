using Sleekflow.Todos.Core.Models;
using Sleekflow.Todos.DAL.Models;

namespace Sleekflow.Todos.Test.Mocks;
public static class MockUsers
{
    public static List<User> GetList()
    {
        return [
            new()
            {
                Id = new Guid("10b0f3fe-2893-4dfe-9448-ab37e8e86d95"),
                UserName = "Exist",
                PasswordHash = "$2a$11$RYSuXkNLiaTraaDu6AyYJuNHPW1JfMAcIiEMgeEh11kQxFGsUOoTm",
                CreatedBy = "Exist",
                UpdatedBy = "Exist",
                IsActive = true,
            },
            new()
            {
                Id = new Guid("10b0f3fe-2893-4dfe-9448-ab37e8e86d35"),
                UserName = "Suspended",
                PasswordHash = "$2a$11$RYSuXkNLiaTraaDu6AyYJuNHPW1JfMAcIiEMgeEh11kQxFGsUOoTm",
                CreatedBy = "Suspended",
                UpdatedBy = "Suspended",
                IsActive = false
            }
        ];
    }
}