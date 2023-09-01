using CSharpFunctionalExtensions;
using WebGLives.Core;

namespace WebGLives.BusinessLogic.Services.Abstract;

public interface IGamesService
{
    Task<Result<IEnumerable<Game>>> All(CancellationToken token = default);
    Task<Result> Create(Game newGame, CancellationToken token = default);
    Task<Result<Game>> Get(int id, CancellationToken token = default);
    Task<Result> Update(int id, Game updated, CancellationToken token = default);
    Task<Result> Delete(int id, CancellationToken token = default);
}