using CSharpFunctionalExtensions;
using WebGLives.BusinessLogic.Services.Abstract;
using WebGLives.Core;
using WebGLives.Core.Errors;
using WebGLives.Core.Repositories;

namespace WebGLives.BusinessLogic.Services;

public class GamesService : IGamesService
{
    private readonly IGamesRepository _repository;

    public GamesService(IGamesRepository repository) =>
        _repository = repository;

    public async Task<Result<IEnumerable<Game>, Error>> All(CancellationToken token = default) =>
        await _repository.All(token);

    public async Task<UnitResult<Error>> Create(Game newGame, CancellationToken token = default) =>
        await _repository.Create(newGame, token);

    public async Task<Result<Game, Error>> Get(int gameId, CancellationToken token = default) =>
        await _repository.GetOrDefault(gameId, token);

    public async Task<UnitResult<Error>> Update(Game updated, CancellationToken token = default) =>
        await _repository.Update(updated, token);

    public async Task<UnitResult<Error>> Delete(int gameId, CancellationToken token = default) =>
        await _repository.Delete(gameId, token);
}