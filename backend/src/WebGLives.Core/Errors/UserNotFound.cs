namespace WebGLives.Core.Errors;

public class UserNotFound : Error
{
    public UserNotFound(int userId) : this(userId.ToString()) { }
    
    public UserNotFound(string userName) : base($"User with {userName} login not found!") { }
}