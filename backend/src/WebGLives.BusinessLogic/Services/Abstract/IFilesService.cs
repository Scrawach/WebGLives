using CSharpFunctionalExtensions;
using WebGLives.Core.Errors;

namespace WebGLives.BusinessLogic.Services.Abstract;

public interface IFilesService
{
    Task<Result<string, Error>> SaveIcon(string title, Stream icon);
    Task<Result<string, Error>> SaveGame(string title, Stream game);
}