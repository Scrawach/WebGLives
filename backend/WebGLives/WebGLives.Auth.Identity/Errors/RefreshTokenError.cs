using WebGLives.Core.Errors;

namespace WebGLives.Auth.Identity.Errors;

public class RefreshTokenError : Error
{
    public RefreshTokenError() : base("Invalid refresh token!") { }
}