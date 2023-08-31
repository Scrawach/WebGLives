using WebGLives.Core;

namespace WebGLives.DataAccess.Repositories;

public interface IGamePageRepository
{
    int Add(Game page);
    IEnumerable<Game> All();
    Game GetById(int pageId);
}