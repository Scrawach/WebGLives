using System.Text;
using WebGLives.API.Extensions;
using WebGLives.API.Services.Abstract;

namespace WebGLives.API.Services;

public class FilesService : IFilesService
{
    private static readonly string BaseDirectory = Path.Combine(Path.GetTempPath(), "WebGLives", "games");

    private readonly IZipService _zipService;

    public FilesService(IZipService zipService) =>
        _zipService = zipService;

    public async Task<(string gamePath, string posterPath)> SaveGame(string title, IFormFile game, IFormFile icon)
    {
        var root = GetOrCreateRootDirectory(title);
        
        var filePath = Path.Combine(root, $"{title}.zip");
        var iconFileName = $"{title}.{Path.GetExtension(icon.FileName)}";
        var iconPath = Path.Combine(root, iconFileName);
        
        await game.CopyToAsync(filePath);
        await icon.CopyToAsync(iconPath);

        if (_zipService.IsValid(filePath))
            _zipService.Extract(filePath, root);

        return 
        (
            CombinePath("games", $"{title}", $"{Path.GetFileNameWithoutExtension(game.FileName)}", "index.html"), 
            CombinePath("games", $"{title}", $"{iconFileName}")
        );
    }
    
    private static string CombinePath(params string[] directories)
    {
        var builder = new StringBuilder();
        foreach (var directory in directories) 
            builder.Append($"{Path.AltDirectorySeparatorChar}{directory}");
        return builder.ToString();
    }
    
    private static string FileExtensionFrom(string fullName) => 
        fullName.Split('.').Last();

    private static string GetOrCreateRootDirectory(string directoryName)
    {
        var root = Path.Combine(BaseDirectory, directoryName);
        if (DirectoryNotExist(root))
            CreateDirectory(root);
        return root;
    }
    
    private static bool DirectoryNotExist(string path) => 
        !Directory.Exists(path);

    private static DirectoryInfo CreateDirectory(string path) => 
        Directory.CreateDirectory(path);
}