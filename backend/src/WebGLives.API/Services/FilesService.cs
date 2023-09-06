using System.Text;
using CSharpFunctionalExtensions;
using WebGLives.API.Extensions;
using WebGLives.API.Services.Abstract;

namespace WebGLives.API.Services;

public class FilesService : IFilesService
{
    private static readonly string BaseDirectory = Path.Combine(Path.GetTempPath(), "WebGLives", "games");

    private readonly IZipService _zipService;

    public FilesService(IZipService zipService) =>
        _zipService = zipService;

    public async Task<Result<string>> SaveIcon(string title, IFormFile icon)
    {
        var root = GetOrCreateRootDirectory(title);
        var fileName = $"{title}.{Path.GetExtension(icon.FileName)}";
        var path = Path.Combine(root, fileName);
        
        await icon.CopyToAsync(path);
        
        return CombinePath("games", $"{title}", $"{fileName}");
    }

    public async Task<Result<string>> SaveGame(string title, IFormFile game)
    {
        var root = GetOrCreateRootDirectory(title);
        var path = Path.Combine(root, $"{title}.zip");
        await game.CopyToAsync(path);

        if (!_zipService.IsValid(path))
            return Result.Failure<string>("Not valid zip archive!");
        
        _zipService.Extract(path, root);
        return CombinePath("games", $"{title}", $"{Path.GetFileNameWithoutExtension(game.FileName)}", "index.html");
    }

    private static string CombinePath(params string[] directories)
    {
        var builder = new StringBuilder();
        
        foreach (var directory in directories) 
            builder.Append($"{Path.AltDirectorySeparatorChar}{directory}");
        
        return builder.ToString();
    }
    
    private static string GetOrCreateRootDirectory(string directoryName)
    {
        var root = Path.Combine(BaseDirectory, directoryName);
        
        if (!Directory.Exists(root))
            Directory.CreateDirectory(root);
        
        return root;
    }
}