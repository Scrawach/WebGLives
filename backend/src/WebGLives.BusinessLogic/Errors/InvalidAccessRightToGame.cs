using WebGLives.Core.Errors;

namespace WebGLives.BusinessLogic.Errors;

public class InvalidAccessRightToGame : Error
{
    public InvalidAccessRightToGame(string username, int gameId) 
        : base($"{username} han not access rights to game #{gameId}") { }
}