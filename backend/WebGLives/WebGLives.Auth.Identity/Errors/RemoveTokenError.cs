using WebGLives.Core.Errors;

namespace WebGLives.Auth.Identity.Errors;

public class RemoveTokenError : Error
{
    public RemoveTokenError(string username) : base($"Remove token error for {username}") { }
}