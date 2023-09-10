using Microsoft.AspNetCore.Mvc;
using WebGLives.API.Requests;
using WebGLives.BusinessLogic.Services.Abstract;
using WebGLives.Core;

namespace WebGLives.API.Controllers;

[ApiController]
[Route("[controller]")]
public class GamesController : FunctionalControllerBase
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
        var result = await _games.All();
        return ResponseFrom(result);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Game))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    public async Task<IActionResult> Get(int id)
    {
        var result = await _games.Get(id);
        return ResponseFrom(result);
    }
    
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    public async Task<IActionResult> Create([FromForm] UploadGameRequest request)
    {
        var game = await GameFrom(request);
        var createdResult = await _games.Create(game);
        return ResponseFrom(createdResult);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    public async Task<IActionResult> Update(int id, [FromForm] UpdateGameRequest request)
    {
        var game = await _games.Get(id);

        if (game.IsFailure)
            return ResponseFrom(game.Error);

        if (request.Title != null)
            game.Value.Title = request.Title;

        if (request.Description != null)
            game.Value.Description = request.Description;

        if (request.Game != null)
        {
            var gamePath = await _files.SaveGame(game.Value.Title, request.Game.OpenReadStream());

            if (gamePath.IsFailure)
                return ResponseFrom(gamePath.Error);
            
            game.Value.GameUrl = gamePath.Value;
        }

        if (request.Icon != null)
        {
            var iconPath = await _files.SaveIcon(game.Value.Title, request.Icon.OpenReadStream());

            if (iconPath.IsFailure)
                return ResponseFrom(iconPath.Error);

            game.Value.PosterUrl = iconPath.Value;
        }

        var updated = await _games.Update(game.Value);
        return ResponseFrom(updated);
    }

    [HttpPut("{id:int}/title")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    public async Task<IActionResult> UpdateTitle(int id, string title)
    {
        var result = await _games.UpdateTitle(id, title);
        return ResponseFrom(result);
    }
    
    [HttpPut("{id:int}/description")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    public async Task<IActionResult> UpdateDescription(int id, string description)
    {
        var result = await _games.UpdateDescription(id, description);
        return ResponseFrom(result);
    }
    
    [HttpPut("{id:int}/poster")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    public async Task<IActionResult> UpdatePoster(int id, IFormFile poster)
    {
        return Forbid();
    }
    
    [HttpPut("{id:int}/game")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    public async Task<IActionResult> UpdateGame(int id, IFormFile archive)
    {
        return Forbid();
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _games.Delete(id);
        return ResponseFrom(result);
    }

    private async Task<Game> GameFrom(UploadGameRequest request)
    {
        var gamePath = await _files.SaveGame(request.Title, request.Game.OpenReadStream());
        var posterPath = await _files.SaveIcon(request.Title, request.Icon.OpenReadStream());
        return new Game { Title = request.Title, Description = request.Description, GameUrl = gamePath.Value, PosterUrl = posterPath.Value };
    }
}