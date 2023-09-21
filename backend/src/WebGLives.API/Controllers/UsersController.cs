using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebGLives.API.Contracts;

namespace WebGLives.API.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController : FunctionalControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;

    public UsersController(UserManager<IdentityUser> userManager) =>
        _userManager = userManager;

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GameResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    public async Task<IActionResult> CreateUser([FromForm] CreateUserRequest request)
    {
        var user = new IdentityUser(request.Login);
        var result = await _userManager.CreateAsync(user, request.Password);
        return result.Succeeded
            ? Ok()
            : BadRequest(result.Errors);
    }
}