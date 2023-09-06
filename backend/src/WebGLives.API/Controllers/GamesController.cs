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
    public async Task<ActionResult> Create([FromForm] UploadGameRequest request)
    {
        var game = await GameFrom(request);
        var created = await _games.Create(game);
        return created.IsSuccess ? Ok() : BadRequest(created.Error);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    public async Task<ActionResult> Update(int id, [FromForm] UpdateGameRequest request)
    {
        var game = await _games.Get(id);

        if (game.IsFailure)
            return BadRequest(game.Error);

        if (request.Title != null)
            game.Value.Title = request.Title;

        if (request.Description != null)
            game.Value.Description = request.Description;

        if (request.Game != null)
        {
            var gamePath = await _files.SaveGame(game.Value.Title, request.Game);

            if (gamePath.IsFailure)
                return BadRequest(gamePath.Error);
            
            game.Value.GameUrl = gamePath.Value;
        }

        if (request.Icon != null)
        {
            var iconPath = await _files.SaveIcon(game.Value.Title, request.Icon);

            if (iconPath.IsFailure)
                return BadRequest(iconPath.Error);

            game.Value.PosterUrl = iconPath.Value;
        }

        var updated = await _games.Update(id, game.Value);
        return updated.IsSuccess ? Ok() : BadRequest(updated.Error);
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _games.Delete(id);
        return result.IsSuccess ? Ok() : BadRequest(result.Error);
    }

    private async Task<Game> GameFrom(UploadGameRequest request)
    {
        var gamePath = await _files.SaveGame(request.Title, request.Game);
        var posterPath = await _files.SaveIcon(request.Title, request.Icon);
        return new Game { Title = request.Title, Description = request.Description, GameUrl = gamePath.Value, PosterUrl = posterPath.Value };
    }
}