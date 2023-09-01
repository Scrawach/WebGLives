using CSharpFunctionalExtensions;
using WebGLives.Core;
using WebGLives.DataAccess.Repositories;

namespace WebGLives.API.Services;

public class GamesService : IGamesService
{
    private readonly IGamesRepository _repository;

    public GamesService(IGamesRepository repository) =>
        _repository = repository;

    public async Task<Result<IEnumerable<Game>>> All()
    {
        var games = await _repository.All();
        return Result.Success(games);
    }

    public async Task<Result> Create(Game newGame)
    {
        var isCreated = await _repository.Create(newGame);
        return isCreated ? Result.Success() : Result.Failure($"Game {newGame.Id} not created");
    }

    public async Task<Result<Game>> Get(int id)
    {
        var game = await _repository.GetOrDefault(id);
        return game is not null ? Result.Success(game) : Result.Failure<Game>($"Game {id} not found!");
    }

    public async Task<Result> Delete(int id)
    {
        var isDeleted = await _repository.Delete(id);
        return isDeleted ? Result.Success() : Result.Failure($"Game {id} not deleted!");
    }
}