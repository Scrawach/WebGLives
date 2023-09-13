using CSharpFunctionalExtensions;
using WebGLives.Core;
using WebGLives.Core.Errors;

namespace WebGLives.BusinessLogic.Services.Abstract;

public interface IGamesService
{
    Task<Result<IEnumerable<Game>, Error>> All(CancellationToken token = default);
    Task<Result<Game, Error>> Create(CancellationToken token = default);
    Task<Result<Game, Error>> Get(int gameId, CancellationToken token = default);
    Task<UnitResult<Error>> Update(int gameId, UpdateGameData data, CancellationToken token = default);
    Task<UnitResult<Error>> UpdateTitle(int gameId, string title, CancellationToken token = default);
    Task<UnitResult<Error>> UpdateDescription(int gameId, string description, CancellationToken token = default);
    Task<UnitResult<Error>> UpdateGame(int gameId, Stream gameStream, CancellationToken token = default);
    Task<UnitResult<Error>> UpdatePoster(int gameId, Stream posterStream, CancellationToken token = default);
    Task<UnitResult<Error>> Delete(int gameId, CancellationToken token = default);
}