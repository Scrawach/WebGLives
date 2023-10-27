using WebGLives.Core;

namespace WebGLives.BusinessLogic.Policies;

public class UserOwnGamePolicy : IGameAccessPolicy
{
    public bool HasAccess(IUser user, Game game) =>
        game.UserId == user.Id;
}