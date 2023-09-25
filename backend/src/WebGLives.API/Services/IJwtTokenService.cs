using System.Security.Claims;
using CSharpFunctionalExtensions;

namespace WebGLives.API.Services;

public interface IJwtTokenService
{
    Result<string> GenerateAccessToken(params Claim[] claims);
    Result<IEnumerable<Claim>> Decode(string accessToken);
}