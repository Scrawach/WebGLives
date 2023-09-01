using Microsoft.AspNetCore.Mvc;
using WebGLives.API.Requests;
using WebGLives.API.Services;
using WebGLives.Core;

namespace WebGLives.API.Controllers;

[ApiController]
[Route("[controller]")]
public class GamesController : ControllerBase
{
    private readonly IFilesService _files;
    private readonly IGamesService _games;

    public GamesController(IFilesService files, IGamesService games)
    {
        _files = files;
        _games = games;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Game>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> All()
    {
        var games = await _games.All();

        if (games.IsSuccess)
            return Ok(games.Value);
        
        return BadRequest(games.Error);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Game))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> Get(int id)
    {
        var result = await _games.Get(id);

        if (result.IsSuccess)
            return Ok(result.Value);

        return BadRequest(result.Error);
    }
    
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    public async Task<ActionResult> Create([FromForm] GameRequest request)
    {
        var (gamePath, posterPath) = await _files.SaveGame(request.Title, request.Game, request.Icon);
        var result = await _games.Create(new Game { Title = request.Title, Description = request.Description, GameUrl = gamePath, PosterUrl = posterPath});
        return result.IsSuccess
            ? Ok()
            : BadRequest(result.Error);
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _games.Delete(id);

        return result.IsSuccess
            ? Ok()
            : BadRequest(result.Error);
    }
}