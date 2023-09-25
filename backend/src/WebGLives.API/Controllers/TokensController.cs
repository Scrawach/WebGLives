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
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuthenticatedResponse))]
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

        var response = await Login(user);
        return Ok(response);
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    public async Task<IActionResult> Decode(string token)
    {
        var claims = _jwtToken.Decode(token);
        return claims.IsSuccess
            ? Ok(claims.Value.Identity.Name)
            : BadRequest(claims.Error);
    }

    [HttpPut("refresh")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuthenticatedResponse))]
    public async Task<IActionResult> Refresh([FromForm] TokenRefreshRequest request)
    {
        var principalClaims = _jwtToken.DecodeExpired(request.AccessToken);

        if (principalClaims.IsFailure)
            return BadRequest("Invalid access token!");
        
        var username = principalClaims.Value.Identity.Name;
        var user = await _userManager.FindByNameAsync(username);

        if (user is null)
            return BadRequest("Invalid token refresh request!");

        var refreshToken = await _userManager.GetAuthenticationTokenAsync(user, "LocalLogin", "RefreshToken");

        if (request.RefreshToken != refreshToken)
            return BadRequest("Invalid refresh token!");

        var response = await Login(user);
        return Ok(response);
    }

    private async Task<AuthenticatedResponse> Login(IdentityUser user)
    {
        var newAccessToken = _jwtToken.GenerateAccessToken
        (
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Name, user.UserName!)
        );
        
        await _userManager.RemoveAuthenticationTokenAsync(user, "LocalLogin", "RefreshToken");
        var newRefreshToken = _jwtToken.GenerateRefreshToken();
        await _userManager.SetAuthenticationTokenAsync(user, "LocalLogin", "RefreshToken", newRefreshToken.Value);

        return new AuthenticatedResponse
        {
            AccessToken = newAccessToken.Value,
            RefreshToken = newRefreshToken.Value
        };
    }
    
}