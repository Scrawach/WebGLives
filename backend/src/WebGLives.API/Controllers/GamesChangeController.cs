using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebGLives.API.Contracts.Games;
using WebGLives.API.Extensions;
using WebGLives.BusinessLogic.Services.Abstract;

namespace WebGLives.API.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class GamesChangeController : FunctionalControllerBase
{
    private readonly IGamesChangeService _games;
    
    public GamesChangeController(IGamesChangeService games) => 
        _games = games;

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GameResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    public async Task<IActionResult> Create(CancellationToken token = default) =>
        await AuthorizedResponseFromAsync(userId => _games.Create(userId, token));

    [HttpPut("{gameId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]   
    public async Task<IActionResult> Update(int gameId, [FromForm] UpdateGameRequest request, CancellationToken token = default)
    {
        await using var updatedData = request.ToData();
        return await AuthorizedResponseFromAsync(userId => _games.Update(userId, gameId, updatedData, token));
    }

    [HttpPut("{gameId:int}/title")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    public async Task<IActionResult> UpdateTitle(int gameId, [FromBody] string title, CancellationToken token = default) =>
        await AuthorizedResponseFromAsync(userId => _games.UpdateTitle(userId, gameId, title, token));

    [HttpPut("{gameId:int}/description")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    public async Task<IActionResult> UpdateDescription(int gameId, [FromBody] string description, CancellationToken token = default) =>
        await AuthorizedResponseFromAsync(userId => _games.UpdateDescription(userId, gameId, description, token));

    [HttpPut("{gameId:int}/poster")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    public async Task<IActionResult> UpdatePoster(int gameId, IFormFile poster, CancellationToken token = default)
    {
        await using var updatedData = poster.ToData();
        return await AuthorizedResponseFromAsync(userId => _games.UpdatePoster(userId, gameId, updatedData, token));
    }

    [HttpPut("{gameId:int}/game")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    public async Task<IActionResult> UpdateGame(int gameId, IFormFile archive, CancellationToken token = default)
    {
        await using var updatedData = archive.ToData();
        return await AuthorizedResponseFromAsync(userId => _games.UpdateGame(userId, gameId, updatedData, token));
    }

    [HttpDelete("{gameId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    public async Task<IActionResult> Delete(int gameId, CancellationToken token = default) =>
        await AuthorizedResponseFromAsync(userId => _games.Delete(userId, gameId, token));
}