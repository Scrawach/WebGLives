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
        await AsyncResponseFrom(GameResponse.From(_games.Create(UserId, token)));

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]   
    public async Task<IActionResult> Update(int id, [FromForm] UpdateGameRequest request, CancellationToken token = default)
    {
        await using var updatedData = request.ToData();
        return await AsyncResponseFrom(_games.Update(UserId, id, updatedData, token));
    }

    [HttpPut("{id:int}/title")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    public async Task<IActionResult> UpdateTitle(int id, [FromBody] string title, CancellationToken token = default) =>
        await AsyncResponseFrom(_games.UpdateTitle(UserId, id, title, token));

    [HttpPut("{id:int}/description")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    public async Task<IActionResult> UpdateDescription(int id, [FromBody] string description, CancellationToken token = default) =>
        await AsyncResponseFrom(_games.UpdateDescription(UserId, id, description, token));

    [HttpPut("{id:int}/poster")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    public async Task<IActionResult> UpdatePoster(int id, IFormFile poster, CancellationToken token = default)
    {
        await using var updatedData = poster.ToData();
        return await AsyncResponseFrom(_games.UpdatePoster(UserId, id, updatedData, token));
    }

    [HttpPut("{id:int}/game")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    public async Task<IActionResult> UpdateGame(int id, IFormFile archive, CancellationToken token = default)
    {
        await using var updatedData = archive.ToData();
        return await AsyncResponseFrom(_games.UpdateGame(UserId, id, updatedData, token));
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    public async Task<IActionResult> Delete(int id, CancellationToken token = default) =>
        await AsyncResponseFrom(_games.Delete(UserId, id, token));
}