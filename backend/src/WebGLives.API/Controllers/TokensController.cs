using System.Security.Claims;
using CSharpFunctionalExtensions;
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

        await _userManager.RemoveAuthenticationTokenAsync(user, "LocalLogin", "RefreshToken");
        var newRefreshToken = _jwtToken.GenerateRefreshToken();
        await _userManager.SetAuthenticationTokenAsync(user, "LocalLogin", "RefreshToken", newRefreshToken.Value);

        var token = _jwtToken.GenerateAccessToken
        (
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Name, user.UserName!)
        );

        var response = new AuthenticatedResponse
        {
            AccessToken = token.Value,
            RefreshToken = newRefreshToken.Value
        };
        
        return Ok(response);
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
    public async Task<IActionResult> Decode(string token)
    {
        var claims = _jwtToken.Decode(token);
        if (claims.IsSuccess)
            return Ok(claims.Value.Identity.Name);
        return BadRequest(claims.Error);
    }

    [HttpPut("refresh")]
    public async Task<IActionResult> Refresh(TokenRefreshRequest request)
    {
        var principalClaims = _jwtToken.DecodeExpired(request.AccessToken);
        var username = principalClaims.Value.Identity.Name;
        var user = await _userManager.FindByNameAsync(username);

        if (user is null)
            return BadRequest("Invalid token refresh request!");

        var refreshToken = await _userManager.GetAuthenticationTokenAsync(user, "LocalLogin", "RefreshToken");

        if (request.RefreshToken != refreshToken)
            return BadRequest("Invalid refresh token!");
        
        var newAccessToken = _jwtToken.GenerateAccessToken
        (
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Name, user.UserName!)
        );
        await _userManager.RemoveAuthenticationTokenAsync(user, "LocalLogin", "RefreshToken");
        var newRefreshToken = _jwtToken.GenerateRefreshToken();
        await _userManager.SetAuthenticationTokenAsync(user, "LocalLogin", "RefreshToken", newRefreshToken.Value);
        
        var response = new AuthenticatedResponse
        {
            AccessToken = newAccessToken.Value,
            RefreshToken = newRefreshToken.Value
        };
        
        return Ok(response);
    }
    
}