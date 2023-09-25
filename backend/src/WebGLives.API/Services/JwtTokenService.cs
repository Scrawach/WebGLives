using System.Security.Claims;
using JWT.Algorithms;
using JWT.Builder;
using WebGLives.API.Controllers;
using WebGLives.API.Extensions;

namespace WebGLives.API.Services;

public class JwtTokenService : IJwtTokenService
{
    private const string Secret = "without secrets";
    
    private readonly IJwtAlgorithm _algorithm = new HMACSHA256Algorithm();
    private readonly long _expirationTime = DateTimeOffset.UtcNow.AddHours(1).ToUnixTimeSeconds();

    public string GenerateAccessToken(params Claim[] claims) =>
        JwtBuilder.Create()
            .WithAlgorithm(_algorithm)
            .WithSecret(Secret)
            .ExpirationTime(_expirationTime)
            .WithVerifySignature(true)
            .AddClaims(claims)
            .WithVerifySignature(true)
            .Encode();

    public IEnumerable<Claim> Decode(string accessToken) =>
        JwtBuilder.Create()
            .WithAlgorithm(_algorithm)
            .WithSecret(Secret)
            .MustVerifySignature()
            .Decode<IDictionary<string, object>>(accessToken)
            .Select(claimData => new Claim(claimData.Key, claimData.Value.ToString() ?? string.Empty));
}