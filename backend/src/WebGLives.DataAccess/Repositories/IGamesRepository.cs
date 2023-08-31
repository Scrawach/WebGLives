using WebGLives.Core;

namespace WebGLives.DataAccess.Repositories;

public interface IGamesRepository
{
    int Add(Game page);
    IEnumerable<Game> All();
    Game GetById(int pageId);
}