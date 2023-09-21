using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebGLives.API.Contracts;

namespace WebGLives.API.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController : FunctionalControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;

    public UsersController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(IEnumerable<IdentityError>))]
    public async Task<IActionResult> CreateUser([FromForm] CreateUserRequest request)
    {
        var user = new IdentityUser(request.Login);
        var result = await _userManager.CreateAsync(user, request.Password);
        return result.Succeeded
            ? Ok()
            : BadRequest(result.Errors);
    }

    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Microsoft.AspNetCore.Identity.SignInResult))]
    public async Task<IActionResult> Login([FromForm] LoginRequest request)
    {
        var result = await _signInManager.PasswordSignInAsync(request.Login, request.Password, request.RememberMe, true);
        return Ok(result);
    }

    [HttpPost("logout")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return Ok();
    }
}