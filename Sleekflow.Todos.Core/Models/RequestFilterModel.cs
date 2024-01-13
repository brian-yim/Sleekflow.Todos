namespace Sleekflow.Todos.Core.Models;

public class RequestFilterModel
{
    public required string Field { get; set; }
    public required object Value { get; set; }
}