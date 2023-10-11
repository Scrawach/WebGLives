using System.Security.Claims;
using JWT.Builder;

namespace WebGLives.Auth.Identity.Extensions;

public static class JwtBuilderExtensions
{
    public static JwtBuilder AddClaims(this JwtBuilder builder, IEnumerable<Claim> claims)
    {
        foreach (var claim in claims) 
            builder.AddClaim(claim.Type, claim.Value);
        return builder;
    }
}