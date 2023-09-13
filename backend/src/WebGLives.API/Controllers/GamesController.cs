using Microsoft.AspNetCore.Mvc;
using WebGLives.API.Extensions;
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
    public async Task<IActionResult> All() =>
        await AsyncResponseFrom(_games.All());

    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Game))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    public async Task<IActionResult> Get(int id) =>
        await AsyncResponseFrom(_games.Get(id));

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    public async Task<IActionResult> Create() =>
        await AsyncResponseFrom(_games.Create());

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]   
    public async Task<IActionResult> Update(int id, [FromBody] UpdateGameRequest request) =>
        await AsyncResponseFrom(_games.Update(id, request.ToData()));
    
    [HttpPut("{id:int}/title")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    public async Task<IActionResult> UpdateTitle(int id, string title) =>
        await AsyncResponseFrom(_games.UpdateTitle(id, title));

    [HttpPut("{id:int}/description")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    public async Task<IActionResult> UpdateDescription(int id, string description) =>
        await AsyncResponseFrom(_games.UpdateDescription(id, description));

    [HttpPut("{id:int}/poster")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    public async Task<IActionResult> UpdatePoster(int id, IFormFile poster) =>
        await AsyncResponseFrom(_games.UpdatePoster(id, poster.OpenReadStream()));

    [HttpPut("{id:int}/game")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    public async Task<IActionResult> UpdateGame(int id, IFormFile archive) =>
        await AsyncResponseFrom(_games.UpdateGame(id, archive.OpenReadStream()));

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    public async Task<IActionResult> Delete(int id) =>
        await AsyncResponseFrom(_games.Delete(id));
}