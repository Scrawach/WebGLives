using WebGLives.Core;

namespace WebGLives.DataAccess.Repositories;

public interface IGamesRepository
{
    Task<IEnumerable<Game>> All(CancellationToken token = default);
    Task<bool> Create(Game game, CancellationToken token = default);
    Task<Game?> GetOrDefault(int id, CancellationToken token = default);
    Task<bool> Update(Game game, CancellationToken token = default);
    Task<bool> Delete(int id, CancellationToken token = default);
}