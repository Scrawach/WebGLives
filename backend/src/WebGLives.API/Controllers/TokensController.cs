using Microsoft.AspNetCore.Mvc;
using WebGLives.API.Contracts;
using WebGLives.API.Services;

namespace WebGLives.API.Controllers;

[ApiController]
[Route("[controller]")]
public class TokensController : FunctionalControllerBase
{
    private readonly ITokenFactory _tokens;

    public TokensController(ITokenFactory tokens) =>
        _tokens = tokens;

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuthenticatedResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    public async Task<IActionResult> Create([FromForm] LoginRequest request) =>
        await AsyncResponseFrom(_tokens.Create(request));

    [HttpPut("refresh")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuthenticatedResponse))]
    public async Task<IActionResult> Refresh([FromForm] TokenRefreshRequest request) =>
        await AsyncResponseFrom(_tokens.Refresh(request));
}