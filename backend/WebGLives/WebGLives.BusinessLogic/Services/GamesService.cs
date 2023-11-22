using CSharpFunctionalExtensions;
using WebGLives.BusinessLogic.Errors;
using WebGLives.BusinessLogic.Extensions;
using WebGLives.BusinessLogic.Services.Abstract;
using WebGLives.Core;
using WebGLives.Core.Errors;
using WebGLives.Core.Repositories;

namespace WebGLives.BusinessLogic.Services;

public class GamesService : IGamesCatalogService, IGamesUpdateService
{
    private readonly IGamesRepository _games;
    private readonly IUsersRepository _users;
    private readonly IGameAccessService _access;
    private readonly IFilesService _files;

    public GamesService(IGamesRepository games, IUsersRepository users, IGameAccessService access, IFilesService files)
    {
        _games = games;
        _users = users;
        _access = access;
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
        await GameWithAccessCheck(userId, gameId, token)
            .TapIf(data.Title is not null, game => game.Title = data.Title)
            .TapIf(data.Description is not null, game => game.Description = data.Description)
            .CheckAndSavePosterFile(data.Poster, _files, token)
            .CheckAndSaveGameFile(data.Game, _files, token)
            .Check(game => _games.Update(game, token));

    public async Task<UnitResult<Error>> Delete(int userId, int gameId, CancellationToken token = default) =>
        await GameWithAccessCheck(userId, gameId, token)
            .Check(_ => _files.Delete(gameId.ToString()))
            .Check(_ => _games.Delete(gameId, token));

    public async Task<UnitResult<Error>> UpdateTitle(int userId, int gameId, string title, CancellationToken token = default) =>
        await GameWithAccessCheck(userId, gameId, token)
            .Tap(game => game.Title = title)
            .Check(game => _games.Update(game, token));

    public async Task<UnitResult<Error>> UpdateDescription(int userId, int gameId, string description, CancellationToken token = default) =>
        await GameWithAccessCheck(userId, gameId, token)
            .Tap(game => game.Description = description)
            .Check(game => _games.Update(game, token));

    public async Task<UnitResult<Error>> UpdateGame(int userId, int gameId, FileData file, CancellationToken token = default) =>
        await GameWithAccessCheck(userId, gameId, token)
            .CheckAndSaveGameFile(file, _files, token)
            .Check(game => _games.Update(game, token));

    public async Task<UnitResult<Error>> UpdatePoster(int userId, int gameId, FileData file, CancellationToken token = default) =>
        await GameWithAccessCheck(userId, gameId, token)
            .CheckAndSavePosterFile(file, _files, token)
            .Check(game => _games.Update(game, token));

    private Task<Result<Game, Error>> GameWithAccessCheck(int userId, int gameId, CancellationToken token) =>
        _games.GetOrDefault(gameId, token)
            .Check(game => _access.HasAccess(userId, game));
}