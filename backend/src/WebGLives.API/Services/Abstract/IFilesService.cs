using CSharpFunctionalExtensions;
using WebGLives.Core.Errors;

namespace WebGLives.API.Services.Abstract;

public interface IFilesService
{
    Task<Result<string, Error>> SaveIcon(string title, IFormFile icon);
    Task<Result<string, Error>> SaveGame(string title, IFormFile game);
}