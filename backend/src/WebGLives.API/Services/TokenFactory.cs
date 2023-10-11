using System.Security.Claims;
using CSharpFunctionalExtensions;
using WebGLives.API.Contracts;
using WebGLives.Core.Errors;
using WebGLives.Core.Repositories;
using WebGLives.Core.Users;

namespace WebGLives.API.Services;

public class TokenFactory : ITokenFactory
{
    private readonly IUsersRepository _users;
    private readonly IJwtTokenService _jwtTokenService;

    public TokenFactory(IUsersRepository users, IJwtTokenService jwtTokenService)
    {
        _users = users;
        _jwtTokenService = jwtTokenService;
    }
    
    public async Task<Result<AuthenticatedResponse, Error>> Create(string login, string password) =>
        await _users
            .FindByNameAsync(login)
            .Tap(async user => await _users.CheckPasswordAsync(user, password))
            .Map(async user => await Authentication(user));
    
    public async Task<Result<AuthenticatedResponse, Error>>  Refresh(string accessToken, string refreshToken) =>
        await _jwtTokenService
            .DecodeExpired(accessToken)
                .Ensure(claims => claims.Identity is not null, new Error("Invalid claims identity!"))
                .Ensure(claims => claims.Identity!.Name is not null, new Error("Invalid claims username!"))
                .Map(claims => claims.Identity!.Name!)
            .Map(username => _users.FindByNameAsync(username))
            .Map(user => (user: user.Value, refreshToken: refreshToken))
            .Ensure(IsValidRefreshToken, new Error("Invalid refresh token!"))
            .Map(async login => await Authentication(login.user));

    private async Task<bool> IsValidRefreshToken((IUser user, string refreshToken) data)
    {
        var oldRefreshToken = await _users.GetAuthenticationTokenAsync(data.user);
        return data.refreshToken == oldRefreshToken.Value;
    }
    
    private async Task<AuthenticatedResponse> Authentication(IUser user)
    {
        await _users.RemoveAuthenticationTokenAsync(user);
        var (accessToken, refreshToken) = GenerateTokens(user);
        await _users.SetAuthenticationTokenAsync(user, refreshToken);

        return new AuthenticatedResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
    }

    private (string access, string refresh) GenerateTokens(IUser user) =>
        (
            _jwtTokenService.GenerateAccessToken
            (
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName!)
            ), 
            _jwtTokenService.GenerateRefreshToken()
        );
}