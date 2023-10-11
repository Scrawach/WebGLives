using System.Security.Claims;

namespace WebGLives.Auth.Identity.Services;

public interface IJwtTokenService
{
    string GenerateAccessToken(params Claim[] claims);
    string GenerateRefreshToken();
}