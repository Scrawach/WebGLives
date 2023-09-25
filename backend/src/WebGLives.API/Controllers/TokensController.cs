using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebGLives.API.Contracts;
using WebGLives.API.Services;

namespace WebGLives.API.Controllers;

[ApiController]
[Route("[controller]")]
public class TokensController : FunctionalControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IJwtTokenService _jwtToken;

    public TokensController(UserManager<IdentityUser> userManager, IJwtTokenService jwtToken)
    {
        _userManager = userManager;
        _jwtToken = jwtToken;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    public async Task<IActionResult> Create([FromForm] LoginRequest request)
    {
        var user = await _userManager.FindByNameAsync(request.Login);

        if (user == null)
            return NotFound($"User with this login not found");

        var isSuccess = await _userManager.CheckPasswordAsync(user, request.Password);

        if (!isSuccess)
            return BadRequest("Invalid password");


        var token = _jwtToken.GenerateAccessToken
        (
            new Claim(ClaimTypes.NameIdentifier, user.Id)
        );

        return Ok(token);
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
    public async Task<IActionResult> Decode(string token)
    {
        var user = User;
        var claims = _jwtToken.Decode(token);
        return Ok(claims);
    }
}