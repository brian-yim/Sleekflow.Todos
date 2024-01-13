namespace Sleekflow.Todos.Core.Models;

public class TodoModel
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public DateTime DueDate { get; set; }
}