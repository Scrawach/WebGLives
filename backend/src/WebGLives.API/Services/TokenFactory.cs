using System.Security.Claims;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using WebGLives.API.Contracts;
using WebGLives.Core.Errors;

namespace WebGLives.API.Services;

public class TokenFactory : ITokenFactory
{
    private const string LocalProvider = "LocalLogin";
    private const string RefreshTokenName = "RefreshToken";
    
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IJwtTokenService _jwtTokenService;

    public TokenFactory(UserManager<IdentityUser> userManager, IJwtTokenService jwtTokenService)
    {
        _userManager = userManager;
        _jwtTokenService = jwtTokenService;
    }
    
    public async Task<Result<AuthenticatedResponse, Error>> Create(string login, string password) =>
        await _userManager
            .FindByNameAsync(login)
            .ToResultAsync(new Error("User with this login not found!"))
            .Ensure(async user => await _userManager.CheckPasswordAsync(user, password), new Error("Invalid password!"))
            .Map(async user => await Authentication(user));
    
    public async Task<Result<AuthenticatedResponse, Error>>  Refresh(string accessToken, string refreshToken) =>
        await _jwtTokenService
            .DecodeExpired(accessToken)
                .Ensure(claims => claims.Identity is not null, new Error("Invalid claims identity!"))
                .Ensure(claims => claims.Identity!.Name is not null, new Error("Invalid claims username!"))
                .Map(claims => claims.Identity!.Name!)
            .Map(username => _userManager.FindByNameAsync(username))
                .Ensure(user => user is not null, new NotFoundError("User with this login not found!"))
                .Map(user => (user: user!, refreshToken: refreshToken))
            .Ensure(IsValidRefreshToken, new Error("Invalid refresh token!"))
            .Map(async login => await Authentication(login.user));

    private async Task<bool> IsValidRefreshToken((IdentityUser user, string refreshToken) data)
    {
        var oldRefreshToken = await _userManager.GetAuthenticationTokenAsync(data.user, LocalProvider, RefreshTokenName);
        return data.refreshToken == oldRefreshToken;
    }
    
    private async Task<AuthenticatedResponse> Authentication(IdentityUser user)
    {
        await _userManager.RemoveAuthenticationTokenAsync(user, LocalProvider, RefreshTokenName);
        var (accessToken, refreshToken) = GenerateTokens(user);
        await _userManager.SetAuthenticationTokenAsync(user, LocalProvider, RefreshTokenName, refreshToken);

        return new AuthenticatedResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
    }

    private (string access, string refresh) GenerateTokens(IdentityUser user) =>
        (
            _jwtTokenService.GenerateAccessToken
            (
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName!)
            ), 
            _jwtTokenService.GenerateRefreshToken()
        );
}