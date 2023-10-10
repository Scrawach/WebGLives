using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using CSharpFunctionalExtensions;
using JWT;
using JWT.Algorithms;
using JWT.Builder;
using JWT.Exceptions;
using WebGLives.API.Extensions;
using WebGLives.Core.Errors;

namespace WebGLives.API.Services;

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
        CreateJwtBuilder()
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

    public Result<ClaimsPrincipal, Error> DecodeExpired(string accessToken) =>
        Decode(accessToken, ValidationParameters.Default
            .With(parameter => parameter.ValidateExpirationTime = false));

    private Result<ClaimsPrincipal, Error> Decode(string token, ValidationParameters parameters)
    {
        try
        {
            var claims = CreateJwtBuilder()
                .MustVerifySignature()
                .WithValidationParameters(parameters)
                .Decode<IDictionary<string, object>>(token)
                .Select(claimData => new Claim(claimData.Key, claimData.Value.ToString() ?? string.Empty))
                .ToArray();

            return Result.Success<ClaimsPrincipal, Error>(new ClaimsPrincipal(new ClaimsIdentity(claims)));
        }
        catch (TokenNotYetValidException)
        {
            return Result.Failure<ClaimsPrincipal, Error>(new Error("Token is not valid yet!"));
        }
        catch (TokenExpiredException)
        {
            return Result.Failure<ClaimsPrincipal, Error>(new Error("Token has expired!"));
        }
        catch (SignatureVerificationException)
        {            
            return Result.Failure<ClaimsPrincipal, Error>(new Error("Token has invalid signature!"));
        }
        catch (Exception)
        {
            return Result.Failure<ClaimsPrincipal, Error>(new Error("Token has not valid format!"));
        }
    }

    private JwtBuilder CreateJwtBuilder() =>
        JwtBuilder.Create()
            .WithAlgorithm(_algorithm)
            .WithSecret(_secret);
}