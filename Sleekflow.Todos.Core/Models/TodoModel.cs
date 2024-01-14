using System.ComponentModel.DataAnnotations;

namespace Sleekflow.Todos.Core.Models;

public class TodoModel
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public DateTime DueDate { get; set; }
    public int Priority { get; set; }
    
    [EnumDataType(typeof(Enums.Status))]
    public string Status { get; set; } = nameof(Enums.Status.NotStarted);
    public IEnumerable<string> Tags { get; set; } = [];
}