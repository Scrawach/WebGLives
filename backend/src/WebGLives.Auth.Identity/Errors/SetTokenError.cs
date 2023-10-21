using WebGLives.Core.Errors;

namespace WebGLives.Auth.Identity.Errors;

public class SetTokenError : Error
{
    public SetTokenError(string username) : base($"Set token error for {username}") { }
}