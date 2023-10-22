using CSharpFunctionalExtensions;
using WebGLives.Core;
using WebGLives.Core.Errors;

namespace WebGLives.BusinessLogic.Services.Abstract;

public interface IGameAccessService
{
    Task<Result<bool, Error>> HasAccess(int userId, Game game);
}