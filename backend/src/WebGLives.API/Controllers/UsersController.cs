using Microsoft.AspNetCore.Mvc;
using WebGLives.API.Contracts.Users;
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
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    public async Task<IActionResult> CreateUser([FromForm] CreateUserRequest request) =>
        await AsyncResponseFrom(_users.CreateAsync(request.Login, request.Password));

    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get(int id) =>
        await AsyncResponseFrom(UserResponse.From(_users.FindByIdAsync(id)));
}