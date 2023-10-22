using WebGLives.Core;

namespace WebGLives.BusinessLogic.Policies;

public class AdminPolicy : IGameAccessPolicy
{
    public bool HasAccess(IUser user, Game game) =>
        IsAdmin(user);

    private bool IsAdmin(IUser user) =>
        false;
}