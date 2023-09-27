using System.Security.Claims;
using CSharpFunctionalExtensions;
using WebGLives.Core.Errors;

namespace WebGLives.API.Services;

public interface IJwtTokenService
{
    string GenerateAccessToken(params Claim[] claims);
    string GenerateRefreshToken();
    Result<ClaimsPrincipal, Error> Decode(string accessToken);
    Result<ClaimsPrincipal, Error> DecodeExpired(string accessToken);
}