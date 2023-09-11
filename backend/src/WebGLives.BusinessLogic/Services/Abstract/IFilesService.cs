using CSharpFunctionalExtensions;
using WebGLives.Core.Errors;

namespace WebGLives.BusinessLogic.Services.Abstract;

public interface IFilesService
{
    Task<Result<string, Error>> SaveIcon(string title, Stream icon, CancellationToken token = default);
    Task<Result<string, Error>> SaveGame(string title, Stream game, CancellationToken token = default);
    UnitResult<Error> Delete(string title);
}