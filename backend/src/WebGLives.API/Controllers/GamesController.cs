using Microsoft.AspNetCore.Mvc;
using WebGLives.API.Requests;
using WebGLives.API.Services.Abstract;
using WebGLives.BusinessLogic.Services.Abstract;
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
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    public async Task<IActionResult> All()
    {
        var games = await _games.All();
        return games.IsSuccess ? Ok(games.Value) : BadRequest(games.Error);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Game))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    public async Task<IActionResult> Get(int id)
    {
        var game = await _games.Get(id);
        return game.IsSuccess ? Ok(game.Value) : BadRequest(game.Error);
    }
    
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    public async Task<ActionResult> Create([FromForm] GameRequest request)
    {
        var game = await GameFrom(request);
        var created = await _games.Create(game);
        return created.IsSuccess ? Ok() : BadRequest(created.Error);
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    public async Task<ActionResult> Update(int id, [FromForm] GameRequest request)
    {
        var game = await GameFrom(request);
        await _games.Update(id, game);
        return Ok();
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _games.Delete(id);
        return result.IsSuccess ? Ok() : BadRequest(result.Error);
    }

    private async Task<Game> GameFrom(GameRequest request)
    {
        var (gamePath, posterPath) = await _files.SaveGame(request.Title, request.Game, request.Icon);
        return new Game { Title = request.Title, Description = request.Description, GameUrl = gamePath, PosterUrl = posterPath };
    }
}