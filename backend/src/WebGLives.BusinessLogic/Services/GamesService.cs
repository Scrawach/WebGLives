using CSharpFunctionalExtensions;
using WebGLives.BusinessLogic.Errors;
using WebGLives.BusinessLogic.Services.Abstract;
using WebGLives.Core;
using WebGLives.Core.Errors;
using WebGLives.Core.Repositories;

namespace WebGLives.BusinessLogic.Services;

public class GamesService : IGamesService, IGamesChangeService
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

    public async Task<Result<Game, Error>> Create(int userId, CancellationToken token = default) =>
        await _users
            .FindByIdAsync(userId)
            .Bind(async user => await _games.Create(new Game() {UserId = user.Id}, token));

    public async Task<Result<Game, Error>> Get(int gameId, CancellationToken token = default) =>
        await _games.GetOrDefault(gameId, token);

    public async Task<UnitResult<Error>> Update(int userId, int gameId, UpdateGameData data, CancellationToken token = default) =>
        await _games.GetOrDefault(gameId, token)
            .Ensure(game => game.UserId == userId, new InvalidAccessRightToGame(userId, gameId))
            .TapIf(data.Title is not null, game => game.Title = data.Title)
            .TapIf(data.Description is not null, game => game.Description = data.Description)
            .CheckIf(data.Poster is not null, async game => await _files
                .Save(gameId.ToString(), data.Poster!, token)
                .Tap(path => game.PosterUrl = path))
            .CheckIf(data.Game is not null, async game => await _files
                .SaveZip(gameId.ToString(), data.Game!, token)
                .Tap(path => game.GameUrl = path))
            .Check(game => _games.Update(game, token));

    public async Task<UnitResult<Error>> Delete(int userId, int gameId, CancellationToken token = default) =>
        await _games.GetOrDefault(gameId, token)
            .Ensure(game => true, new InvalidAccessRightToGame(userId, gameId))
            .Check(_ => _files.Delete(gameId.ToString()))
            .Check(_ => _games.Delete(gameId, token));

    public async Task<UnitResult<Error>> UpdateTitle(int userId, int gameId, string title, CancellationToken token = default) =>
        await _games.GetOrDefault(gameId, token)
            .Ensure(game => game.UserId == userId, new InvalidAccessRightToGame(userId, gameId))
            .Tap(game => game.Title = title)
            .Check(game => _games.Update(game, token));

    public async Task<UnitResult<Error>> UpdateDescription(int userId, int gameId, string description, CancellationToken token = default) =>
        await _games.GetOrDefault(gameId, token)
            .Ensure(game => game.UserId == userId, new InvalidAccessRightToGame(userId, gameId))
            .Tap(game => game.Description = description)
            .Check(game => _games.Update(game, token));

    public async Task<UnitResult<Error>> UpdateGame(int userId, int gameId, FileData file, CancellationToken token = default) =>
        await _games.GetOrDefault(gameId, token)
            .Ensure(game => game.UserId == userId, new InvalidAccessRightToGame(userId, gameId))
            .Check(async game => await _files
                .SaveZip(gameId.ToString(), file, token)
                .Tap(path => game.GameUrl = path))
            .Check(game => _games.Update(game, token));

    public async Task<UnitResult<Error>> UpdatePoster(int userId, int gameId, FileData file, CancellationToken token = default) =>
        await _games.GetOrDefault(gameId, token)
            .Ensure(game => game.UserId == userId, new InvalidAccessRightToGame(userId, gameId))
            .Check(async game => await _files
                .Save(gameId.ToString(), file, token)
                .Tap(path => game.PosterUrl = path))
            .Check(game => _games.Update(game, token));
    
}