namespace WebGLives.BusinessLogic;

public class UpdateGameData : IDisposable, IAsyncDisposable
{
    public string? Title;
    public string? Description;
    public Stream? Poster;
    public Stream? Game;

    public void Dispose()
    {
        Poster?.Dispose();
        Game?.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        if (Poster is not null)
            await Poster.DisposeAsync();

        if (Game is not null)
            await Game.DisposeAsync();
    }
}