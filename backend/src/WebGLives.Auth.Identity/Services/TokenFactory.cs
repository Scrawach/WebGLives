using System.Security.Claims;
using CSharpFunctionalExtensions;
using WebGLives.Core.Errors;
using WebGLives.Core.Users;

namespace WebGLives.Auth.Identity.Services;

public class TokenFactory : ITokenFactory
{
    private readonly IUsersService _users;
    private readonly IJwtTokenService _jwtTokenService;

    public TokenFactory(IUsersService users, IJwtTokenService jwtTokenService)
    {
        _users = users;
        _jwtTokenService = jwtTokenService;
    }
    
    public async Task<Result<Tokens, Error>> Create(string login, string password) =>
        await _users.FindByNameAsync(login)
            .Check(async entity => await _users.CheckPasswordAsync(entity, password))
            .Map(async user => await Authentication(user));

    public async Task<Result<Tokens, Error>> Refresh(string username, string refreshToken) =>
        await _users.FindByNameAsync(username)
            .Map(user => (user, refreshToken))
            .Ensure(IsValidRefreshToken, new Error("Invalid refresh token!"))
            .Map(async login => await Authentication(login.user));

    private async Task<bool> IsValidRefreshToken((IUser user, string refreshToken) data)
    {
        var oldRefreshToken = await _users.GetAuthenticationTokenAsync(data.user);
        return data.refreshToken == oldRefreshToken.Value;
    }
    
    private async Task<Tokens> Authentication(IUser user)
    {
        await _users.RemoveAuthenticationTokenAsync(user);
        var (accessToken, refreshToken) = GenerateTokens(user);
        await _users.SetAuthenticationTokenAsync(user, refreshToken);

        return new Tokens
        {
            Access = accessToken,
            Refresh = refreshToken
        };
    }

    private (string access, string refresh) GenerateTokens(IUser user) =>
        (
            _jwtTokenService.GenerateAccessToken
            (
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName!)
            ), 
            _jwtTokenService.GenerateRefreshToken()
        );
}