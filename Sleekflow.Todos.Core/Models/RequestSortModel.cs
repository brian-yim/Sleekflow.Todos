namespace Sleekflow.Todos.Core.Models;

public class RequestSortModel
{
    public required string Field { get; set; }
    public string Direction { get; set; } = "+";
}