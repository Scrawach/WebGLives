using System.Security.Claims;
using CSharpFunctionalExtensions;
using WebGLives.Auth.Identity.Errors;
using WebGLives.Core;
using WebGLives.Core.Errors;
using WebGLives.Core.Repositories;

namespace WebGLives.Auth.Identity.Services;

public class TokenFactory : ITokenFactory
{
    private readonly IUsersRepository _users;
    private readonly IRefreshTokensRepository _refreshTokens;
    private readonly IJwtTokenService _jwtTokenService;

    public TokenFactory(IUsersRepository users, IRefreshTokensRepository refreshTokens, IJwtTokenService jwtTokenService)
    {
        _users = users;
        _jwtTokenService = jwtTokenService;
        _refreshTokens = refreshTokens;
    }

    public async Task<Result<Tokens, Error>> Create(string login, string password) =>
        await _users.FindByNameAsync(login)
            .CheckPassword(_users, password)
            .Authenticate(CreateTokens);

    public async Task<Result<Tokens, Error>> Refresh(int userId, string refreshToken) =>
        await _users.FindByIdAsync(userId)
            .Map(user => (user, refreshToken))
            .Ensure(IsValidRefreshToken, new RefreshTokenError())
            .Map(async login => await CreateTokens(login.user));

    private async Task<bool> IsValidRefreshToken((IUser user, string refreshToken) data)
    {
        var oldRefreshToken = await _refreshTokens.GetToken(data.user);
        return data.refreshToken == oldRefreshToken.Value;
    }
    
    private async Task<Tokens> CreateTokens(IUser user)
    {
        await _refreshTokens.RemoveToken(user);
        var expireAt = DateTime.Now.AddMinutes(1);
        var (accessToken, refreshToken) = GenerateTokens(user, expireAt);
        await _refreshTokens.CreateToken(user, refreshToken);

        return new Tokens
        {
            Access = accessToken,
            Refresh = refreshToken,
            ExpireAt = expireAt
        };
    }

    private (string access, string refresh) GenerateTokens(IUser user, DateTime expireAt) =>
        (
            _jwtTokenService.GenerateAccessToken
            (
                expireAt,
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName!)
            ), 
            _jwtTokenService.GenerateRefreshToken()
        );
}