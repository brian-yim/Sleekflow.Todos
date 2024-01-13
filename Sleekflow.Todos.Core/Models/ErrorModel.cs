namespace Sleekflow.Todos.Core.Models;

public class ErrorModel
{
    public string? Message { get; set; }
    public ErrorModel(string message)
    {
        Message = message;
    }
}

public class NotFoundError : ErrorModel
{
    public NotFoundError(string message = "Record Not Found.")
    : base(message) { }
}