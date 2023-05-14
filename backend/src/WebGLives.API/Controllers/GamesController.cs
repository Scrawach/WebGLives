using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace WebGLives.API.Controllers;

[ApiController]
[Route("api/games")]
public class GamesController : Controller
{
    private readonly ILogger<GamesController> _logger;
    private static readonly string BaseDirectory = Path.Combine(Path.GetTempPath(), "games");

    public GamesController(ILogger<GamesController> logger) => 
        _logger = logger;

    [HttpGet]
    [Route("{gameName}")]
    public IActionResult Index(string gameName)
    {
        if (DirectoryNotExist())
            CreateDirectory();
        
        var pathToGame = Path.Combine(BaseDirectory, gameName);

        if (Directory.Exists(pathToGame))
            return GamePage(pathToGame);
        
        return HelloWorldPage();
    }

    private IActionResult GamePage(string pathToGame)
    {
        var filePath = Path.Combine(pathToGame, "index.html");
        var fileBytes = System.IO.File.ReadAllBytes(filePath);
        return File(fileBytes, "application/octet-stream");
    }

    private static ContentResult HelloWorldPage() =>
        new ContentResult()
        {
            ContentType = "text/html",
            StatusCode = (int)HttpStatusCode.OK,
            Content = "<html><body>Hello World</body></html>"
        };

    private static bool DirectoryNotExist() => 
        !Directory.Exists(BaseDirectory);

    private static DirectoryInfo CreateDirectory() => 
        Directory.CreateDirectory(BaseDirectory);
}