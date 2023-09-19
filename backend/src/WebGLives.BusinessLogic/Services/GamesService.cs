using CSharpFunctionalExtensions;
using WebGLives.BusinessLogic.Services.Abstract;
using WebGLives.Core;
using WebGLives.Core.Errors;
using WebGLives.Core.Repositories;

namespace WebGLives.BusinessLogic.Services;

public class GamesService : IGamesService
{
    private readonly IGamesRepository _repository;
    private readonly IFilesService _files;

    public GamesService(IGamesRepository repository, IFilesService files)
    {
        _repository = repository;
        _files = files;
    }

    public async Task<Result<IEnumerable<Game>, Error>> All(CancellationToken token = default) =>
        await _repository.All(token);

    public async Task<Result<Game, Error>> Create(CancellationToken token = default) =>
        await _repository.Create(new Game(), token);

    public async Task<Result<Game, Error>> Get(int gameId, CancellationToken token = default) =>
        await _repository.GetOrDefault(gameId, token);

    public async Task<UnitResult<Error>> Update(int gameId, UpdateGameData data, CancellationToken token = default) =>
        await _repository.GetOrDefault(gameId, token)
            .TapIf(data.Title is not null, game => game.Title = data.Title)
            .TapIf(data.Description is not null, game => game.Description = data.Description)
            .CheckIf(data.Poster is not null, async game => await _files
                .SaveIcon(gameId.ToString(), data.Poster!, token)
                .Tap(path => game.PosterUrl = path))
            .CheckIf(data.Game is not null, async game => await _files
                .SaveGame(gameId.ToString(), data.Game!, token)
                .Tap(path => game.GameUrl = path))
            .Tap(game => _repository.Update(game, token));

    public async Task<UnitResult<Error>> Delete(int gameId, CancellationToken token = default) =>
        await _files
            .Delete(gameId.ToString())
            .Tap(() => _repository.Delete(gameId, token));

    public async Task<UnitResult<Error>> UpdateTitle(int gameId, string title, CancellationToken token = default) =>
        await _repository.GetOrDefault(gameId, token)
            .Tap(game => game.Title = title)
            .Tap(game => _repository.Update(game, token));

    public async Task<UnitResult<Error>> UpdateDescription(int gameId, string description, CancellationToken token = default) =>
        await _repository.GetOrDefault(gameId, token)
            .Tap(game => game.Description = description)
            .Tap(game => _repository.Update(game, token));

    public async Task<UnitResult<Error>> UpdateGame(int gameId, Stream gameStream, CancellationToken token = default) =>
        await _repository.GetOrDefault(gameId, token)
            .Check(async game => await _files
                .SaveGame(gameId.ToString(), gameStream, token)
                .Tap(path => game.GameUrl = path))
            .Tap(game => _repository.Update(game, token));

    public async Task<UnitResult<Error>> UpdatePoster(int gameId, Stream posterStream, CancellationToken token = default) =>
        await _repository.GetOrDefault(gameId, token)
            .Check(async game => await _files
                .SaveIcon(gameId.ToString(), posterStream, token)
                .Tap(path => game.PosterUrl = path))
            .Tap(game => _repository.Update(game, token));
    
}