using CSharpFunctionalExtensions;
using WebGLives.Core.Errors;

namespace WebGLives.Core.Repositories;

public interface IGamesRepository
{
    Task<Result<IEnumerable<Game>, Error>> All(CancellationToken token = default);
    Task<UnitResult<Error>> Create(Game game, CancellationToken token = default);
    Task<Result<Game, Error>> GetOrDefault(int gameId, CancellationToken token = default);
    Task<UnitResult<Error>> Update(Game game, CancellationToken token = default);
    Task<UnitResult<Error>> Delete(int gameId, CancellationToken token = default);
}