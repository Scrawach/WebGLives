using System.Text;
using CSharpFunctionalExtensions;
using WebGLives.BusinessLogic.Services.Abstract;
using WebGLives.Core.Errors;

namespace WebGLives.BusinessLogic.Services;

public class FilesService : IFilesService
{
    private static readonly string BaseDirectory = Path.Combine(Path.GetTempPath(), "WebGLives", "games");

    private readonly IZipService _zipService;

    public FilesService(IZipService zipService) =>
        _zipService = zipService;

    public async Task<Result<string, Error>> SaveIcon(string title, Stream icon)
    {
        var root = GetOrCreateRootDirectory(title);
        var fileName = $"{title}.png";
        var path = Path.Combine(root, fileName);

        await icon.CopyToAsync(new FileStream(path, FileMode.Create));
        
        return CombinePath("games", $"{title}", $"{fileName}");
    }

    public async Task<Result<string, Error>> SaveGame(string title, Stream game)
    {
        var root = GetOrCreateRootDirectory(title);
        var path = Path.Combine(root, $"{title}.zip");
        await game.CopyToAsync(new FileStream(path, FileMode.Create));

        if (!_zipService.IsValid(path))
            return Result.Failure<string, Error>(new Error("Not valid zip archive!"));
        
        _zipService.Extract(path, root);
        return CombinePath("games", $"{title}", $"{Path.GetFileNameWithoutExtension(path)}", "index.html");
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