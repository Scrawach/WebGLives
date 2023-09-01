using Microsoft.AspNetCore.Mvc;
using WebGLives.Core;
using WebGLives.DataAccess.Repositories;

namespace WebGLives.API.Controllers;

[ApiController]
[Route("[controller]")]
public class GamesController : ControllerBase
{
    private readonly IGamesRepository _gamesRepository;

    public GamesController(IGamesRepository gamesRepository) =>
        _gamesRepository = gamesRepository;

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Game>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> All()
    {
        var games = await _gamesRepository.All();
        return Ok(games);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Game))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> Get(int id)
    {
        var game = await _gamesRepository.GetOrDefault(id);
        
        return game is null 
            ? NotFound() 
            : Ok(game);
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> Delete(int id)
    {
        await _gamesRepository.Delete(id);
        return Ok();
    }
}