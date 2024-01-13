namespace Sleekflow.Todos.DAL.Models;

public class Todo : Auditable
{
    public Guid Id { get; set; } = new Guid();
    public required string Name { get; set; }
    public required string Description { get; set; }
    public DateTime DueDate { get; set; }
    public required string Status { get; set; }
}