using System.Text;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Options;
using WebGLives.BusinessLogic.Options;
using WebGLives.BusinessLogic.Services.Abstract;
using WebGLives.Core;
using WebGLives.Core.Errors;


namespace WebGLives.BusinessLogic.Services;

public class FilesService : IFilesService
{
    private readonly IZipService _zipService;
    private readonly IOptions<FilesOptions> _options;

    public FilesService(IZipService zipService, IOptions<FilesOptions> options)
    {
        _zipService = zipService;
        _options = options;
    }

    public async Task<Result<string, Error>> Save(string folderName, FileData file, CancellationToken token = default)
    {
        var fileName = $"{folderName}{file.GetExtension()}";
        await SaveFile(folderName, fileName, file.Stream, token);
        return CombinePath(_options.Value.GamesFolder, folderName, fileName);
    }

    public async Task<Result<string, Error>> SaveZip(string folderName, FileData gameArchive, CancellationToken token = default)
    {
        var (root, path) = await SaveFile(folderName, gameArchive.Name, gameArchive.Stream, token);
        
        if (!_zipService.IsValid(path))
            return Result.Failure<string, Error>(new Error("Not valid zip archive!"));
        
        _zipService.Extract(path, root);

        MoveExtractedFolderToRoot(folderName, root);
        return CombinePath(_options.Value.GamesFolder, folderName, Path.GetFileNameWithoutExtension(path), "index.html");
    }

    public UnitResult<Error> Delete(string folderName)
    {
        var root = Path.Combine(_options.Value.BaseDirectory, folderName);
        
        if (Directory.Exists(root))
            Directory.Delete(root, true);
        
        return UnitResult.Success<Error>();
    }

    private async Task<(string rootFolder, string pathToFile)> SaveFile(string title, string fileName, Stream file, CancellationToken token = default)
    {
        var root = GetOrCreateRootDirectory(title);
        var path = Path.Combine(root, fileName);

        await using var fileStream = new FileStream(path, FileMode.Create);
        await file.CopyToAsync(fileStream, token);
        return (root, path);
    }

    private static string CombinePath(params string[] directories)
    {
        var builder = new StringBuilder();
        
        foreach (var directory in directories) 
            builder.Append($"{Path.AltDirectorySeparatorChar}{directory}");
        
        return builder.ToString();
    }

    private string GetOrCreateRootDirectory(string directoryName)
    {
        var root = Path.Combine(_options.Value.BaseDirectory, directoryName);
        
        if (!Directory.Exists(root))
            Directory.CreateDirectory(root);
        
        return root;
    }

    private static void MoveExtractedFolderToRoot(string folderName, string root)
    {
        var extractedFolder = Directory.GetDirectories(root).FirstOrDefault();
        Directory.Move(extractedFolder!, Path.Combine(root, folderName));
    }
}