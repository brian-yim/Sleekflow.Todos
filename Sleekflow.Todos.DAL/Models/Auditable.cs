namespace Sleekflow.Todos.DAL;

public class Auditable
{
    public required string CreatedBy { get; set; }
    public required string UpdatedBy { get; set; }
    public required DateTime CreatedAt { get; set; }
    public required DateTime UpdatedAt { get; set; }
}