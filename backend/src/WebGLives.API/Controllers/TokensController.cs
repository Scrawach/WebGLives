using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebGLives.API.Contracts.Auth;
using WebGLives.Auth.Identity.Services;

namespace WebGLives.API.Controllers;

[ApiController]
[Route(ApiRoutings.Tokens)]
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
        await ResponseFromAsync(AuthenticatedResponse.From(_tokens.Create(request.Login, request.Password)));

    [Authorize]
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuthenticatedResponse))]
    public async Task<IActionResult> Refresh([FromForm] TokenRefreshRequest request) =>
        await AuthorizedResponseFromAsync(userId => AuthenticatedResponse.From(_tokens.Refresh(userId, request.RefreshToken)));
}