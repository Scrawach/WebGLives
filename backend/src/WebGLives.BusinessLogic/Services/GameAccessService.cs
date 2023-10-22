using CSharpFunctionalExtensions;
using WebGLives.BusinessLogic.Errors;
using WebGLives.BusinessLogic.Policies;
using WebGLives.BusinessLogic.Services.Abstract;
using WebGLives.Core;
using WebGLives.Core.Errors;
using WebGLives.Core.Repositories;

namespace WebGLives.BusinessLogic.Services;

public class GameAccessService : IGameAccessService
{
    private readonly IUsersRepository _users;
    private readonly IEnumerable<IGameAccessPolicy> _policies;

    public GameAccessService(IUsersRepository users, IEnumerable<IGameAccessPolicy> policies)
    {
        _users = users;
        _policies = policies;
    }

    public async Task<UnitResult<Error>> HasAccess(int userId, Game game) =>
        await _users
            .FindByIdAsync(userId)
            .Map(user =>
            {
                return _policies.Aggregate(false, (current, policy) => current || policy.HasAccess(user, game))
                    ? UnitResult.Success<Error>()
                    : UnitResult.Failure<Error>(new InvalidAccessRightToGame(user.Id, game.Id));
            });
}