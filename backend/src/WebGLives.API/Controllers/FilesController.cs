using Microsoft.AspNetCore.Mvc;
using WebGLives.API.Extensions;
using WebGLives.API.Requests;
using WebGLives.API.Services;
using WebGLives.Core;
using WebGLives.DataAccess.Repositories;

namespace WebGLives.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FilesController : ControllerBase
{
    private readonly ILogger<FilesController> _logger;
    private readonly IZipService _zipService;
    private readonly IGamesRepository _repository;
    private static readonly string BaseDirectory = Path.Combine(Path.GetTempPath(), "WebGLives", "games");

    public FilesController(ILogger<FilesController> logger, IZipService zipService, IGamesRepository repository)
    {
        _logger = logger;
        _zipService = zipService;
        _repository = repository;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    public async Task<ActionResult> Post([FromForm] UploadGameRequest request)
    {
        var root = RootDirectory(request.Title);

        var filePath = Path.Combine(root, $"{request.Title}.zip");
        var iconPath = Path.Combine(root, $"{request.Title}.{FileExtensionFrom(request.Icon.FileName)}");
        
        await request.Game.CopyToAsync(filePath);
        await request.Icon.CopyToAsync(iconPath);

        if (_zipService.IsValid(filePath))
            _zipService.Extract(filePath, root);
        
        AddGameCard(request);

        return Ok();
    }

    private void AddGameCard(UploadGameRequest request)
    {
        var path = $"http://localhost:5072/games/{request.Title}/{request.Game.FileName.Split('.').First()}/index.html";
        var icon = $"http://localhost:5072/games/{request.Title}/{request.Title}.{FileExtensionFrom(request.Icon.FileName)}";
        var card = new Game
        {
            Title = request.Title,
            Description = request.Description,
            PosterUrl = icon,
            GameUrl = path
        };
        _repository.Create(card);
    }

    private static string FileExtensionFrom(string fullName) => 
        fullName.Split('.').Last();

    private static string RootDirectory(string directoryName)
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