using CSharpFunctionalExtensions;
using WebGLives.BusinessLogic.Services.Abstract;
using WebGLives.Core;
using WebGLives.Core.Repositories;

namespace WebGLives.BusinessLogic.Services;

public class GamesService : IGamesService
{
    private readonly IGamesRepository _repository;

    public GamesService(IGamesRepository repository) =>
        _repository = repository;

    public async Task<Result<IEnumerable<Game>>> All(CancellationToken token = default)
    {
        var games = await _repository.All(token);
        return Result.Success(games);
    }

    public async Task<Result> Create(Game newGame, CancellationToken token = default)
    {
        var isCreated = await _repository.Create(newGame, token);
        return isCreated ? Result.Success() : Result.Failure($"Game {newGame.Id} not created");
    }

    public async Task<Result<Game>> Get(int id, CancellationToken token = default)
    {
        var game = await _repository.GetOrDefault(id, token);
        return game is not null ? Result.Success(game) : Result.Failure<Game>($"Game {id} not found!");
    }

    public async Task<Result> Update(int id, Game updated, CancellationToken token = default)
    {
        var item = await Get(id, token);

        if (item.IsFailure)
            return item;

        updated.Id = id;
        var isUpdated = await _repository.Update(updated, token);
        return isUpdated ? Result.Success() : Result.Failure($"Game {id} not updated!");
    }

    public async Task<Result> Delete(int id, CancellationToken token = default)
    {
        var isDeleted = await _repository.Delete(id, token);
        return isDeleted ? Result.Success() : Result.Failure($"Game {id} not deleted!");
    }
}