using WebGLives.BusinessLogic.Services.Abstract;
using WebGLives.Core;

namespace WebGLives.BusinessLogic.Policies;

public class GameHasLinkOnUser : IGameAccessPolicy
{
    public bool HasAccess(IUser user, Game game) =>
        game.UserId == user.Id;
}