using System.ComponentModel.DataAnnotations;

namespace Sleekflow.Todos.DAL;

public class Auditable
{
    [MaxLength(255)]
    public required string CreatedBy { get; set; }
    [MaxLength(255)]
    public required string UpdatedBy { get; set; }
    public required DateTime CreatedAt { get; set; }
    public required DateTime UpdatedAt { get; set; }
}