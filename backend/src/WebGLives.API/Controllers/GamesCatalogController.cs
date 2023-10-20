using Microsoft.AspNetCore.Mvc;
using WebGLives.API.Contracts.Games;
using WebGLives.BusinessLogic.Services.Abstract;

namespace WebGLives.API.Controllers;

[ApiController]
[Route("[controller]")]
public class GamesCatalogController : FunctionalControllerBase
{
    private readonly IGamesService _games;

    public GamesCatalogController(IGamesService games) =>
        _games = games;
    
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<GameResponse>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    public async Task<IActionResult> All(CancellationToken token = default) =>
        await ResponseFromAsync(GameResponse.From(_games.All(token)));

    [HttpGet("{gameId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GameResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    public async Task<IActionResult> Get(int gameId, CancellationToken token = default) =>
        await ResponseFromAsync(GameResponse.From(_games.Get(gameId, token)));
}