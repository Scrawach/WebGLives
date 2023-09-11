using Microsoft.AspNetCore.Mvc;
using WebGLives.API.Requests;
using WebGLives.BusinessLogic.Services.Abstract;
using WebGLives.Core;

namespace WebGLives.API.Controllers;

[ApiController]
[Route("[controller]")]
public class GamesController : FunctionalControllerBase
{
    private readonly IGamesService _games;

    public GamesController(IGamesService games) =>
        _games = games;

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
    public async Task<IActionResult> Create()
    {
        var createdResult = await _games.Create();
        return ResponseFrom(createdResult);
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
        var result = await _games.UpdatePoster(id, poster.OpenReadStream());
        return ResponseFrom(result);
    }
    
    [HttpPut("{id:int}/game")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    public async Task<IActionResult> UpdateGame(int id, IFormFile archive)
    {
        var result = await _games.UpdateGame(id, archive.OpenReadStream());
        return ResponseFrom(result);
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
}