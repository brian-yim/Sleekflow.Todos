using Microsoft.EntityFrameworkCore;
using Sleekflow.Todos.DAL.Models;

namespace Sleekflow.Todos.DAL;

public class TodoContext : DbContext
{
    public TodoContext()
    {
    }

    public TodoContext(DbContextOptions<TodoContext> options)
        : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer("");
        }
    }

    public DbSet<Todo> Todos { get; set; }
    public DbSet<TodoTag> TodoTags { get; set; }

}
