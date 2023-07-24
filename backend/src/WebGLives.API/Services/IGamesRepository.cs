namespace WebGLives.API.Services;

public interface IGamePagesRepository
{
    public IEnumerable<GameCard> All();
}