using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sleekflow.Todos.Core.Models;
using Sleekflow.Todos.Core.Services;
using Sleekflow.Todos.Web.Extensions;

namespace Sleekflow.Todos.Web.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]s")]
public class TodoController(ITodoService todoService) : ControllerBase
{
    private readonly ITodoService _todoService = todoService;

    [HttpGet]
    public async Task<IActionResult> List(
        [FromQuery] string? sortString,
        [FromQuery] RequestFilterModel[]? filters
    )
    {
        var sort = string.IsNullOrWhiteSpace(sortString) ?
            null :
            new RequestSortModel()
            {
                Field = sortString.Remove(0, 1),
                Direction = $"{sortString[0]}",
            };

        var result = await _todoService.GetAsync(this.GetUserId(), filters, sort);

        return this.ToReponse(result);
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> GetByIdAsync(Guid id)
    {
        var result = await _todoService.GetAsync(id);

        return this.ToReponse(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync(TodoModel model)
    {
        var result = await _todoService.CreateAsync(this.GetUserId(), model);
        return this.ToReponse(result);
    }

    [HttpPut]
    [Route("{id}")]
    public async Task<IActionResult> UpdateAsync(Guid id, TodoModel model)
    {
        var result = await _todoService.UpdateAsync(this.GetUserId(), id, model);
        return this.ToReponse(result);
    }

    [HttpDelete]
    [Route("{id}")]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        var result = await _todoService.DeleteAsync(this.GetUserId(), id);
        return this.ToReponse(result);
    }
}