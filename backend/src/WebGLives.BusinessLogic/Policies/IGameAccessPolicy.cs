using WebGLives.Core;

namespace WebGLives.BusinessLogic.Policies;

public interface IGameAccessPolicy
{
    bool HasAccess(IUser user, Game game);
}