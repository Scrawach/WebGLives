using CSharpFunctionalExtensions;
using WebGLives.Core;
using WebGLives.Core.Errors;

namespace WebGLives.BusinessLogic.Services.Abstract;

public interface IGamesChangeService
{
    Task<Result<Game, Error>> Create(int userId, CancellationToken token = default);
    Task<UnitResult<Error>> Update(int userId, int gameId, UpdateGameData data, CancellationToken token = default);
    Task<UnitResult<Error>> UpdateTitle(int userId, int gameId, string title, CancellationToken token = default);
    Task<UnitResult<Error>> UpdateDescription(int userId, int gameId, string description, CancellationToken token = default);
    Task<UnitResult<Error>> UpdateGame(int userId, int gameId, FileData file, CancellationToken token = default);
    Task<UnitResult<Error>> UpdatePoster(int userId, int gameId, FileData poster, CancellationToken token = default);
    Task<UnitResult<Error>> Delete(int userId, int gameId, CancellationToken token = default);
}