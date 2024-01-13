
namespace Sleekflow.Todos.Core.Helpers;

public class FieldMapper
{
    public static readonly Dictionary<string, string> map = new()
    {
        ["name"] = "Name",
        ["description"] = "Description",
        ["duedate"] = "DueDate",
        ["status"] = "Status",
        ["createdby"] = "CreatedBy",
        ["updatedby"] = "UpdatedBy",
        ["createdat"] = "CreatedAt",
        ["updatedat"] = "UpdatedAt",
    };
}