using CSharpFunctionalExtensions;
using WebGLives.Core;
using WebGLives.Core.Errors;

namespace WebGLives.BusinessLogic.Services.Abstract;

public interface IGamesService
{
    Task<Result<IEnumerable<Game>, Error>> All(CancellationToken token = default);
    Task<UnitResult<Error>> Create(Game newGame, CancellationToken token = default);
    Task<Result<Game, Error>> Get(int id, CancellationToken token = default);
    Task<UnitResult<Error>> Update(int id, Game updated, CancellationToken token = default);
    Task<UnitResult<Error>> Delete(int id, CancellationToken token = default);
}