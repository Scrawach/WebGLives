namespace WebGLives.Core.Errors;

public class UserNotFound : Error
{
    public UserNotFound(string userName) : base($"User with {userName} login not found!") { }
}