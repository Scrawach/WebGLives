using System.Security.Claims;

namespace WebGLives.API.Services;

public interface IJwtTokenService
{
    string GenerateAccessToken(params Claim[] claims);
    IEnumerable<Claim> Decode(string accessToken);
}