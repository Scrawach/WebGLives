using CSharpFunctionalExtensions;
using WebGLives.Core;
using WebGLives.Core.Errors;

namespace WebGLives.BusinessLogic.Services.Abstract;

public interface IAuthorizedGameService
{
    Task<Result<Game, Error>> Create(string username, CancellationToken token = default);
}