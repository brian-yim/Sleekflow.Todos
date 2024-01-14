namespace Sleekflow.Todos.DAL.Models;

public class User : Auditable
{
    public Guid Id { get; set; } = new Guid();
    public required string UserName { get; set; }
    public required string PasswordHash { get; set; }
    public bool IsActive { get; set; }
    public IList<Role>? Roles { get; set; } = [];

}