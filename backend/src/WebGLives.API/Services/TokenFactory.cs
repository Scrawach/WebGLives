using System.Security.Claims;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using WebGLives.API.Contracts;
using WebGLives.Core.Errors;

namespace WebGLives.API.Services;

public class TokenFactory : ITokenFactory
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IJwtTokenService _jwtTokenService;

    public TokenFactory(UserManager<IdentityUser> userManager, IJwtTokenService jwtTokenService)
    {
        _userManager = userManager;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<Result<AuthenticatedResponse, Error>> Create(LoginRequest request)
    {
        var user = await _userManager.FindByNameAsync(request.Login);

        if (user == null)
            return Result.Failure<AuthenticatedResponse, Error>(new NotFoundError("User with this login not found"));

        var isSuccess = await _userManager.CheckPasswordAsync(user, request.Password);

        if (!isSuccess)
            return Result.Failure<AuthenticatedResponse, Error>(new Error("Invalid password"));

        return await Login(user);
    }

    public async Task<Result> Decode(string token) =>
        _jwtTokenService.Decode(token);
    
    public async Task<Result<AuthenticatedResponse, Error>>  Refresh(TokenRefreshRequest request)
    {
        var principalClaims = _jwtTokenService.DecodeExpired(request.AccessToken);

        if (principalClaims.IsFailure)
            return Result.Failure<AuthenticatedResponse, Error>(new Error("Invalid access token!"));
        
        var username = principalClaims.Value.Identity.Name;
        var user = await _userManager.FindByNameAsync(username);

        if (user is null)
            return Result.Failure<AuthenticatedResponse, Error>(new Error("Invalid token refresh request!"));

        var refreshToken = await _userManager.GetAuthenticationTokenAsync(user, "LocalLogin", "RefreshToken");

        if (request.RefreshToken != refreshToken)
            return Result.Failure<AuthenticatedResponse, Error>(new Error("Invalid refresh token!"));

        return await Login(user);
    }

    private async Task<AuthenticatedResponse> Login(IdentityUser user)
    {
        var newAccessToken = _jwtTokenService.GenerateAccessToken
        (
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Name, user.UserName!)
        );
        
        await _userManager.RemoveAuthenticationTokenAsync(user, "LocalLogin", "RefreshToken");
        var newRefreshToken = _jwtTokenService.GenerateRefreshToken();
        await _userManager.SetAuthenticationTokenAsync(user, "LocalLogin", "RefreshToken", newRefreshToken.Value);

        return new AuthenticatedResponse
        {
            AccessToken = newAccessToken.Value,
            RefreshToken = newRefreshToken.Value
        };
    }
}