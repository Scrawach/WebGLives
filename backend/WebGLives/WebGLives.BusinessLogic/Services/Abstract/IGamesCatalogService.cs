using CSharpFunctionalExtensions;
using WebGLives.Core;
using WebGLives.Core.Errors;

namespace WebGLives.BusinessLogic.Services.Abstract;

public interface IGamesCatalogService
{
    Task<Result<IEnumerable<Game>, Error>> All(CancellationToken token = default);
    Task<Result<Game, Error>> Get(int gameId, CancellationToken token = default);
}