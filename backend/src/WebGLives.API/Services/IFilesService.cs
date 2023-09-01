namespace WebGLives.API.Services;

public interface IFilesService
{
    Task<(string gamePath, string posterPath)> SaveGame(string title, IFormFile game, IFormFile icon);
}