using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sleekflow.Todos.Core.Models;
using Sleekflow.Todos.Core.Services;
using Sleekflow.Todos.Web.Extensions;

namespace Sleekflow.Todos.Web.Controllers;

[AllowAnonymous]
[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthService authServiec) : ControllerBase
{
    private readonly IAuthService _authServiec = authServiec;

    /// <summary>
    /// Return a JWT token if user is valid.
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("login")]
    [ProducesResponseType<TokenModel>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> LoginAsync([FromForm] UserModel model)
    {
        var result = await _authServiec.LoginAsync(model);
        return this.ToReponse(result);
    }

    /// <summary>
    /// Signup for user.
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("signup")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]

    public async Task<IActionResult> SignupAsync([FromForm] UserModel model)
    {
        var result = await _authServiec.SignupAsync(model);
        return this.ToReponse(result);
    }
}