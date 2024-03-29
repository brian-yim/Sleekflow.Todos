namespace Sleekflow.Todos.Core.Models;

public class ErrorModel(string message)
{
    public string? Message { get; set; } = message;
}

public class NotFoundError(string message = "Record Not Found.")
    : ErrorModel(message);

public class AuthError(string message = "Username / Password incorrect")
    : ErrorModel(message);