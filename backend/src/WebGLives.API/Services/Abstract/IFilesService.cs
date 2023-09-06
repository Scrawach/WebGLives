using CSharpFunctionalExtensions;

namespace WebGLives.API.Services.Abstract;

public interface IFilesService
{
    Task<Result<string>> SaveIcon(string title, IFormFile icon);
    Task<Result<string>> SaveGame(string title, IFormFile game);
}