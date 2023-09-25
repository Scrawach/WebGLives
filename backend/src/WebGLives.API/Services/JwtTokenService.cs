using System.Security.Claims;
using System.Security.Cryptography;
using CSharpFunctionalExtensions;
using JWT;
using JWT.Algorithms;
using JWT.Builder;
using JWT.Exceptions;
using WebGLives.API.Extensions;

namespace WebGLives.API.Services;

public class JwtTokenService : IJwtTokenService
{
    private const string Secret = "without secrets";
    
    private readonly IJwtAlgorithm _algorithm = new HMACSHA256Algorithm();
    private readonly long _expirationTime = DateTimeOffset.UtcNow.AddHours(1).ToUnixTimeSeconds();

    public Result<string> GenerateAccessToken(params Claim[] claims) =>
        CreateJwtBuilder()
            .ExpirationTime(_expirationTime)
            .WithVerifySignature(true)
            .AddClaims(claims)
            .WithVerifySignature(true)
            .Encode();

    public Result<string> GenerateRefreshToken()
    {
        var randomNumbers = new byte[32];
        using var random = RandomNumberGenerator.Create();
        random.GetBytes(randomNumbers);
        return Convert.ToBase64String(randomNumbers);
    }
    
    public Result<IEnumerable<Claim>> Decode(string accessToken)
    {
        try
        {
            var claims = CreateJwtBuilder()
                .MustVerifySignature()
                .WithValidationParameters(ValidationParameters.Default)
                .Decode<IDictionary<string, object>>(accessToken)
                .Select(claimData => new Claim(claimData.Key, claimData.Value.ToString() ?? string.Empty));
            return Result.Success(claims);
        }
        catch (TokenNotYetValidException)
        {
            return Result.Failure<IEnumerable<Claim>>("Token is not valid yet!");
        }
        catch (TokenExpiredException)
        {
            return Result.Failure<IEnumerable<Claim>>("Token has expired!");
        }
        catch (SignatureVerificationException)
        {            
            return Result.Failure<IEnumerable<Claim>>("Token has invalid signature!");
        }
        catch (Exception)
        {
            return Result.Failure<IEnumerable<Claim>>("Token has not valid format!");
        }
    }

    private JwtBuilder CreateJwtBuilder() =>
        JwtBuilder.Create()
            .WithAlgorithm(_algorithm)
            .WithSecret(Secret);
}