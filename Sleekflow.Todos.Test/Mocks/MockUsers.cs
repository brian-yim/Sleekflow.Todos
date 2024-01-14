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
                UserName = "Exist",
                PasswordHash = "$2a$11$RYSuXkNLiaTraaDu6AyYJuNHPW1JfMAcIiEMgeEh11kQxFGsUOoTm",
                CreatedBy = "Exist",
                UpdatedBy = "Exist",
            }
        ];
    }
}