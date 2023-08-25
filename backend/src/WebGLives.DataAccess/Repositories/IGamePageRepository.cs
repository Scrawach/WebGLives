using WebGLives.Core;

namespace WebGLives.DataAccess.Repositories;

public interface IGamePageRepository
{
    int Add(GamePage page);
    IEnumerable<GamePage> All();
    GamePage GetById(int pageId);
}