namespace WebGLives.API.Services;

public class GamePagesRepository : IGamePagesRepository
{
    public IEnumerable<GamePage> All()
    {
        yield return new GamePage("0", "good game", "none", "about something");
        yield return new GamePage("1", "best game", "none", "about something");
        yield return new GamePage("2", "poor game", "none", "about something");
    }
}