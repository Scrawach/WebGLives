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

    public async Task<Result<string, Error>> SaveIcon(string title, Stream icon, CancellationToken token = default)
    {
        var root = GetOrCreateRootDirectory(title);
        var fileName = $"{title}.png";
        var path = Path.Combine(root, fileName);

        await using var fileStream = new FileStream(path, FileMode.Create);
        await icon.CopyToAsync(fileStream, token);
        
        return CombinePath("games", $"{title}", $"{fileName}");
    }

    public async Task<Result<string, Error>> SaveGame(string title, Stream game, CancellationToken token = default)
    {
        var root = GetOrCreateRootDirectory(title);
        var path = Path.Combine(root, $"{title}.zip");
        
        await using (var fileStream = new FileStream(path, FileMode.Create)) 
            await game.CopyToAsync(fileStream, token);

        if (!_zipService.IsValid(path))
            return Result.Failure<string, Error>(new Error("Not valid zip archive!"));
        
        _zipService.Extract(path, root);
        return CombinePath("games", $"{title}", $"{Path.GetFileNameWithoutExtension(path)}", "index.html");
    }
    
    public UnitResult<Error> Delete(string title)
    {
        var root = GetOrCreateRootDirectory(title);
        
        if (Directory.Exists(root))
            Directory.Delete(root, true);
        
        return UnitResult.Success<Error>();
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