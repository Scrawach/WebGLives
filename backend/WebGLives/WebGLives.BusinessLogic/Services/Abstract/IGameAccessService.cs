using CSharpFunctionalExtensions;
using WebGLives.Core;
using WebGLives.Core.Errors;

namespace WebGLives.BusinessLogic.Services.Abstract;

public interface IGameAccessService
{
    Task<UnitResult<Error>> HasAccess(int userId, Game game);
}