using System.Security.Claims;
using JWT.Algorithms;
using JWT.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebGLives.API.Contracts;

namespace WebGLives.API.Controllers;

[ApiController]
[Route("[controller]")]
public class TokensController : FunctionalControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;

    public TokensController(UserManager<IdentityUser> userManager) =>
        _userManager = userManager;

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

        var token = JwtBuilder.Create()
            .WithAlgorithm(new HMACSHA256Algorithm())
            .WithSecret("Without Secret")
            .ExpirationTime(DateTimeOffset.UtcNow.AddHours(1).ToUnixTimeSeconds())
            .AddClaim(ClaimTypes.NameIdentifier, user.Id)
            .WithVerifySignature(true)
            .Encode();

        return Ok(token);
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
    public async Task<IActionResult> Decode(string token)
    {
        var user = User;
        var json = JwtBuilder.Create()
            .WithAlgorithm(new HMACSHA256Algorithm())
            .WithSecret("Without secret")
            .MustVerifySignature()
            .Decode(token);
        return Ok(json);
    }
}