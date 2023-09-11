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

    public async Task<Result<Game, Error>> Create(CancellationToken token = default)
    {
        var game = await _repository.Create(new Game(), token);
        return game;
    }

    public async Task<UnitResult<Error>> Update(int gameId, string? title, string? description, Stream? posterFile, Stream? gameFile,
        CancellationToken token = default)
    {
        var game = await _repository.GetOrDefault(gameId, token);

        if (game.IsFailure)
            return game.Error;

        if (title is not null)
            game.Value.Title = title;

        if (description is not null)
            game.Value.Description = description;

        if (gameFile is not null)
        {
            var gamePath = await _files.SaveGame(game.Value.Title, gameFile);

            if (gamePath.IsFailure)
                return gamePath.Error;
            
            game.Value.GameUrl = gamePath.Value;
        }

        if (posterFile is not null)
        {
            var iconPath = await _files.SaveIcon(game.Value.Title, posterFile);

            if (iconPath.IsFailure)
                return iconPath.Error;

            game.Value.PosterUrl = iconPath.Value;
        }

        return await _repository.Update(game.Value, token);
    }

    public async Task<UnitResult<Error>> UpdateTitle(int gameId, string title, CancellationToken token = default) =>
        await _repository.GetOrDefault(gameId, token)
            .Tap(game => game.Title = title)
            .Tap(game => _repository.Update(game, token));

    public async Task<UnitResult<Error>> UpdateDescription(int gameId, string description,
        CancellationToken token = default) =>
        await _repository.GetOrDefault(gameId, token)
            .Tap(game => game.Description = description)
            .Tap(game => _repository.Update(game, token));

    public async Task<Result<Game, Error>> Get(int gameId, CancellationToken token = default) =>
        await _repository.GetOrDefault(gameId, token);

    public async Task<UnitResult<Error>> Update(Game updated, CancellationToken token = default) =>
        await _repository.Update(updated, token);

    public async Task<UnitResult<Error>> Delete(int gameId, CancellationToken token = default) =>
        await _repository.Delete(gameId, token);
}