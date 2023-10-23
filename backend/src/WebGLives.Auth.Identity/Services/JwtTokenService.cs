using System.Security.Claims;
using System.Security.Cryptography;
using JWT.Algorithms;
using JWT.Builder;
using WebGLives.Auth.Identity.Extensions;

namespace WebGLives.Auth.Identity.Services;

public class JwtTokenService : IJwtTokenService
{
    private readonly string _secret;
    private readonly IJwtAlgorithm _algorithm;

    public JwtTokenService(string secret, IJwtAlgorithm algorithm)
    {
        _secret = secret;
        _algorithm = algorithm;
    }

    public string GenerateAccessToken(params Claim[] claims) =>
        JwtBuilder.Create()
            .WithAlgorithm(_algorithm)
            .WithSecret(_secret)
            .ExpirationTime(DateTime.Now.AddMinutes(10))
            .WithVerifySignature(true)
            .AddClaims(claims)
            .Encode();

    public string GenerateRefreshToken()
    {
        var randomNumbers = new byte[32];
        using var random = RandomNumberGenerator.Create();
        random.GetBytes(randomNumbers);
        return Convert.ToBase64String(randomNumbers);
    }
}