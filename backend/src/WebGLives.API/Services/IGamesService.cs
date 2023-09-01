using WebGLives.Core;

namespace WebGLives.API.Services;

public interface IGamesService
{
    Task<IEnumerable<Game>> All();
    Task Create(Game newGame);
    Task<Game> Get(int id);
    Task<bool> Delete(int id);
}