namespace Sleekflow.Todos.Core.Models;

public class ServiceResponseModel<T>
{
    public T? Data { get; set; }
    public List<ErrorModel> Errors { get; set; } = [];
}