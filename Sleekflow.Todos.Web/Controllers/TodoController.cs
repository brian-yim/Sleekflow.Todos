using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sleekflow.Todos.Core.Models;
using Sleekflow.Todos.Core.Services;
using Sleekflow.Todos.DAL.Models;
using Sleekflow.Todos.Web.Extensions;

namespace Sleekflow.Todos.Web.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]s")]
public class TodoController(ITodoService todoService) : ControllerBase
{
    private readonly ITodoService _todoService = todoService;

    /// <summary>
    /// Get user's todo list with sorting and filtering.
    /// </summary>
    /// <param name="sortString" example="+createdAt">+Field name represent ascending Field Name; and vice versa</param>
    /// <param name="filters">filters[0].field=priority&amp;filters[0].value=0</param>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType<IEnumerable<Todo>>(StatusCodes.Status200OK)]
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

    /// <summary>
    /// Get todo by id.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("{id}")]
    [ProducesResponseType<Todo>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByIdAsync(Guid id)
    {
        var result = await _todoService.GetAsync(id);

        return this.ToReponse(result);
    }

    /// <summary>
    /// Create a todo item.
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType<Todo>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateAsync(TodoModel model)
    {
        var result = await _todoService.CreateAsync(this.GetUserId(), model);
        return this.ToReponse(result);
    }

    /// <summary>
    /// Update a todo item.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPut]
    [Route("{id}")]
    [ProducesResponseType<Todo>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateAsync(Guid id, TodoModel model)
    {
        var result = await _todoService.UpdateAsync(this.GetUserId(), id, model);
        return this.ToReponse(result);
    }

    /// <summary>
    /// Delete a todo item.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete]
    [Route("{id}")]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        var result = await _todoService.DeleteAsync(this.GetUserId(), id);
        return this.ToReponse(result);
    }
}