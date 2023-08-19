using System.Diagnostics;
using System.IO.Compression;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
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
        if (DirectoryNotExist())
            CreateDirectory();

        var filePath = Path.Combine(BaseDirectory, request.Title);
        
        await using (var stream = new FileStream(filePath, FileMode.Create)) 
            await request.Game.CopyToAsync(stream);

        if (_zipService.IsValid(filePath))
            _zipService.Extract(filePath, Path.Combine(BaseDirectory, "unzip"));
        
        return Ok();
    }

    private static bool DirectoryNotExist() => 
        !Directory.Exists(BaseDirectory);

    private static DirectoryInfo CreateDirectory() => 
        Directory.CreateDirectory(BaseDirectory);
}