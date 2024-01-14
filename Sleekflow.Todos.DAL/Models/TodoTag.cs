using System.ComponentModel.DataAnnotations;

namespace Sleekflow.Todos.DAL.Models;

public class TodoTag
{
    public Guid Id { get; set; } = new Guid();
    [MaxLength(255)]
    public required string Name { get; set; }
}