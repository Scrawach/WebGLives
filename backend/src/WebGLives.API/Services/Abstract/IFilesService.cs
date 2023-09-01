namespace WebGLives.API.Services.Abstract;

public interface IFilesService
{
    Task<(string gamePath, string posterPath)> SaveGame(string title, IFormFile game, IFormFile icon);
}