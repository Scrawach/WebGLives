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

    public Result<string> GenerateAccessToken(params Claim[] claims) =>
        CreateJwtBuilder()
            .ExpirationTime(DateTime.Now.AddMinutes(1))
            .WithVerifySignature(true)
            .AddClaims(claims)
            .Encode();

    public Result<string> GenerateRefreshToken()
    {
        var randomNumbers = new byte[32];
        using var random = RandomNumberGenerator.Create();
        random.GetBytes(randomNumbers);
        return Convert.ToBase64String(randomNumbers);
    }
    
    public Result<ClaimsPrincipal> Decode(string accessToken) =>
        Decode(accessToken, ValidationParameters.Default);

    public Result<ClaimsPrincipal> DecodeExpired(string accessToken) =>
        Decode(accessToken, ValidationParameters.Default
            .With(parameter => parameter.ValidateExpirationTime = false));

    private Result<ClaimsPrincipal> Decode(string token, ValidationParameters parameters)
    {
        try
        {
            var claims = CreateJwtBuilder()
                .MustVerifySignature()
                .WithValidationParameters(parameters)
                .Decode<IDictionary<string, object>>(token)
                .Select(claimData => new Claim(claimData.Key, claimData.Value.ToString() ?? string.Empty))
                .ToArray();

            return Result.Success(new ClaimsPrincipal(new ClaimsIdentity(claims)));
        }
        catch (TokenNotYetValidException)
        {
            return Result.Failure<ClaimsPrincipal>("Token is not valid yet!");
        }
        catch (TokenExpiredException)
        {
            return Result.Failure<ClaimsPrincipal>("Token has expired!");
        }
        catch (SignatureVerificationException)
        {            
            return Result.Failure<ClaimsPrincipal>("Token has invalid signature!");
        }
        catch (Exception)
        {
            return Result.Failure<ClaimsPrincipal>("Token has not valid format!");
        }
    }

    private JwtBuilder CreateJwtBuilder() =>
        JwtBuilder.Create()
            .WithAlgorithm(_algorithm)
            .WithSecret(Secret);
}