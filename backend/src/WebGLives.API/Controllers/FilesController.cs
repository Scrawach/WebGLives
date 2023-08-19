using Microsoft.AspNetCore.Mvc;
using WebGLives.API.Extensions;
using WebGLives.API.Services;

namespace WebGLives.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FilesController : ControllerBase
{
    private readonly ILogger<FilesController> _logger;
    private readonly IZipService _zipService;
    private static readonly string BaseDirectory = Path.Combine(Path.GetTempPath(), "WebGLives", "games");

    public FilesController(ILogger<FilesController> logger, IZipService zipService)
    {
        _logger = logger;
        _zipService = zipService;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    public async Task<ActionResult> Post([FromForm] UploadGameRequest request)
    {
        var root = RootDirectory(request.Title);

        var filePath = Path.Combine(root, $"{request.Title}.zip");
        var iconPath = Path.Combine(root, $"{request.Title}.png");
        
        await request.Game.CopyToAsync(filePath);
        await request.Icon.CopyToAsync(iconPath);

        if (_zipService.IsValid(filePath))
            _zipService.Extract(filePath, root);
        
        return Ok();
    }

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