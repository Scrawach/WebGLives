using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebGLives.API.Contracts.Auth;
using WebGLives.Core.Users;

namespace WebGLives.API.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController : FunctionalControllerBase
{
    private readonly IUserService _users;

    public UsersController(IUserService users) =>
        _users = users;

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(IEnumerable<IdentityError>))]
    public async Task<IActionResult> CreateUser([FromForm] CreateUserRequest request) =>
        await AsyncResponseFrom(_users.CreateAsync(request.Login, request.Password));
}