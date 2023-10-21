using WebGLives.Core.Errors;

namespace WebGLives.BusinessLogic.Errors;

public class InvalidAccessRightToGame : Error
{
    public InvalidAccessRightToGame(int userId, int gameId) 
        : this(userId.ToString(), gameId) { }
    
    public InvalidAccessRightToGame(string username, int gameId) 
        : base($"{username} has not access rights to game #{gameId}") { }
}