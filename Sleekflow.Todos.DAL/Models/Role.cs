namespace Sleekflow.Todos.DAL.Models;

public class Role : Auditable
{
    public Guid Id { get; set; } = new Guid();
    public required string RoleName { get; set; }
    public bool IsActive { get; set; }
}