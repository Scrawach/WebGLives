using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebGLives.API.Contracts.Games;
using WebGLives.API.Extensions;
using WebGLives.BusinessLogic.Services.Abstract;

namespace WebGLives.API.Controllers;

[ApiController]
[Route("[controller]")]
public class GamesController : FunctionalControllerBase
{
    private readonly IGamesService _games;
    
    public GamesController(IGamesService games) => 
        _games = games;

    [HttpPost]
    [Authorize]
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
        return await AsyncResponseFrom(_games.Update(id, updatedData, token));
    }

    [HttpPut("{id:int}/title")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    public async Task<IActionResult> UpdateTitle(int id, [FromBody] string title, CancellationToken token = default) =>
        await AsyncResponseFrom(_games.UpdateTitle(id, title, token));

    [HttpPut("{id:int}/description")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    public async Task<IActionResult> UpdateDescription(int id, [FromBody] string description, CancellationToken token = default) =>
        await AsyncResponseFrom(_games.UpdateDescription(id, description, token));

    [HttpPut("{id:int}/poster")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    public async Task<IActionResult> UpdatePoster(int id, IFormFile poster, CancellationToken token = default)
    {
        await using var updatedData = poster.ToData();
        return await AsyncResponseFrom(_games.UpdatePoster(id, updatedData, token));
    }

    [HttpPut("{id:int}/game")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    public async Task<IActionResult> UpdateGame(int id, IFormFile archive, CancellationToken token = default)
    {
        await using var updatedData = archive.ToData();
        return await AsyncResponseFrom(_games.UpdateGame(id, updatedData, token));
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    public async Task<IActionResult> Delete(int id, CancellationToken token = default) =>
        await AsyncResponseFrom(_games.Delete(UserId, id, token));
}