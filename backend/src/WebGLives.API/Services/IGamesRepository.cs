namespace WebGLives.API.Services;

public interface IGamePagesRepository
{
    IEnumerable<GameCard> All();
    void Create(GameCard card);
    GameCard GetById(string id);
}