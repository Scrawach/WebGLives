namespace WebGLives.Core.Errors;

public class GameNotFoundError : NotFoundError
{
    public GameNotFoundError(int gameId) : base($"Game {gameId} not found") { }
}