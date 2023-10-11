using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebGLives.API.Contracts;
using WebGLives.Core.Repositories;

namespace WebGLives.API.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController : FunctionalControllerBase
{
    private readonly IUsersRepository _users;

    public UsersController(IUsersRepository users) =>
        _users = users;

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(IEnumerable<IdentityError>))]
    public async Task<IActionResult> CreateUser([FromForm] CreateUserRequest request) =>
        await AsyncResponseFrom(_users.CreateAsync(request.Login, request.Password));
}