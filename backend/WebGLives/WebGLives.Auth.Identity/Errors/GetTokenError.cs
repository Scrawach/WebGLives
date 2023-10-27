using WebGLives.Core.Errors;

namespace WebGLives.Auth.Identity.Errors;

public class GetTokenError : Error
{
    public GetTokenError(string username) : base($"Get token error for {username}") { }
}