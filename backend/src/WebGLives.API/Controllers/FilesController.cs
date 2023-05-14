using System.IO.Compression;
using Microsoft.AspNetCore.Mvc;

namespace WebGLives.API.Controllers;

[ApiController]
[Route("api/upload")]
public class FilesController : ControllerBase
{
    private readonly ILogger<FilesController> _logger;
    private static readonly string BaseDirectory = Path.Combine(Path.GetTempPath(), "games");

    public FilesController(ILogger<FilesController> logger) => 
        _logger = logger;

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    public async Task<ActionResult> Post(IFormFile file)
    {
        if (DirectoryNotExist())
            CreateDirectory();

        var filePath = Path.Combine(BaseDirectory, file.FileName);
        await using (var stream = new FileStream(filePath, FileMode.Create))
            await file.CopyToAsync(stream);

        ExtractFilesFrom(filePath);
        return Ok();
    }

    private static void ExtractFilesFrom(string filePath) => 
        ZipFile.ExtractToDirectory(filePath, Path.Combine(BaseDirectory, "unzip"));

    private static bool DirectoryNotExist() => 
        !Directory.Exists(BaseDirectory);

    private static DirectoryInfo CreateDirectory() => 
        Directory.CreateDirectory(BaseDirectory);
}