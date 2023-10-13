using CSharpFunctionalExtensions;
using WebGLives.BusinessLogic.Services.Abstract;
using WebGLives.Core;
using WebGLives.Core.Errors;
using WebGLives.Core.Repositories;

namespace WebGLives.BusinessLogic.Services;

public class AuthorizedGameService : IAuthorizedGameService
{
    private readonly IGamesService _games;
    private readonly IUsersRepository _users;

    public AuthorizedGameService(IGamesService games, IUsersRepository users)
    {
        _games = games;
        _users = users;
    }

    public async Task<Result<Game, Error>> Create(string username, CancellationToken token = default) =>
        await _users
            .FindByNameAsync(username)
            .Bind(async user => await _games.Create(user.Id, token));
}