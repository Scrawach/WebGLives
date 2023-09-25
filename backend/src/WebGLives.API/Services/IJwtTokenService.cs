using System.Security.Claims;
using CSharpFunctionalExtensions;

namespace WebGLives.API.Services;

public interface IJwtTokenService
{
    Result<string> GenerateAccessToken(params Claim[] claims);
    Result<string> GenerateRefreshToken();
    Result<ClaimsPrincipal> Decode(string accessToken);
    Result<ClaimsPrincipal> DecodeExpired(string accessToken);
}