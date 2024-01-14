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

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> LoginAsync([FromForm] UserModel model)
    {
        var result = await _authServiec.LoginAsync(model);
        return this.ToReponse(result);
    }

    [HttpPost]
    [Route("signup")]
    public async Task<IActionResult> SignupAsync([FromForm] UserModel model)
    {
        var result = await _authServiec.SignupAsync(model);
        return this.ToReponse(result);
    }
}