using System.ComponentModel.DataAnnotations;

namespace Sleekflow.Todos.DAL;

public class Auditable
{
    [MaxLength(255)]
    public required string CreatedBy { get; set; }
    [MaxLength(255)]
    public required string UpdatedBy { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}