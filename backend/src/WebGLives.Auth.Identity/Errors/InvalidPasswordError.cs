using WebGLives.Core.Errors;

namespace WebGLives.Auth.Identity.Errors;

public class InvalidPasswordError : Error
{
    public InvalidPasswordError() : base("Invalid password!") { }
}