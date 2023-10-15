using CSharpFunctionalExtensions;
using WebGLives.BusinessLogic.Services.Abstract;
using WebGLives.Core;
using WebGLives.Core.Errors;
using WebGLives.Core.Repositories;

namespace WebGLives.BusinessLogic.Services;

public class GamesService : IGamesService
{
    private readonly IGamesRepository _games;
    private readonly IUsersRepository _users;
    private readonly IFilesService _files;

    public GamesService(IGamesRepository games, IUsersRepository users, IFilesService files)
    {
        _games = games;
        _users = users;
        _files = files;
    }

    public async Task<Result<IEnumerable<Game>, Error>> All(CancellationToken token = default) =>
        await _games.All(token);

    public async Task<Result<Game, Error>> Create(string username, CancellationToken token = default) =>
        await _users
            .FindByNameAsync(username)
            .Bind(async user => await _games.Create(new Game() {UserId = user.Id}, token));

    public async Task<Result<Game, Error>> Get(int gameId, CancellationToken token = default) =>
        await _games.GetOrDefault(gameId, token);

    public async Task<UnitResult<Error>> Update(int gameId, UpdateGameData data, CancellationToken token = default) =>
        await _games.GetOrDefault(gameId, token)
            .TapIf(data.Title is not null, game => game.Title = data.Title)
            .TapIf(data.Description is not null, game => game.Description = data.Description)
            .CheckIf(data.Poster is not null, async game => await _files
                .Save(gameId.ToString(), data.Poster!, token)
                .Tap(path => game.PosterUrl = path))
            .CheckIf(data.Game is not null, async game => await _files
                .SaveZip(gameId.ToString(), data.Game!, token)
                .Tap(path => game.GameUrl = path))
            .Check(game => _games.Update(game, token));

    public async Task<UnitResult<Error>> Delete(string username, int gameId, CancellationToken token = default) =>
        await _users
            .FindByNameAsync(username)
            .Check(async user => await _games.GetOrDefault(gameId, token)
                .Ensure(game => game.UserId == user.Id, new Error($"{username} has not access to game {gameId}")))
            .Check(_ => _files.Delete(gameId.ToString()))
            .Check(_ => _games.Delete(gameId, token));

    public async Task<UnitResult<Error>> UpdateTitle(int gameId, string title, CancellationToken token = default) =>
        await _games.GetOrDefault(gameId, token)
            .Tap(game => game.Title = title)
            .Check(game => _games.Update(game, token));

    public async Task<UnitResult<Error>> UpdateDescription(int gameId, string description, CancellationToken token = default) =>
        await _games.GetOrDefault(gameId, token)
            .Tap(game => game.Description = description)
            .Check(game => _games.Update(game, token));

    public async Task<UnitResult<Error>> UpdateGame(int gameId, FileData file, CancellationToken token = default) =>
        await _games.GetOrDefault(gameId, token)
            .Check(async game => await _files
                .SaveZip(gameId.ToString(), file, token)
                .Tap(path => game.GameUrl = path))
            .Check(game => _games.Update(game, token));

    public async Task<UnitResult<Error>> UpdatePoster(int gameId, FileData file, CancellationToken token = default) =>
        await _games.GetOrDefault(gameId, token)
            .Check(async game => await _files
                .Save(gameId.ToString(), file, token)
                .Tap(path => game.PosterUrl = path))
            .Check(game => _games.Update(game, token));
    
}