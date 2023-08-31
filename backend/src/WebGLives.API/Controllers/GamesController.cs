using Microsoft.AspNetCore.Mvc;
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
    public async Task<IActionResult> All()
    {
        var games = await _gamesRepository.All();
        return Ok(games);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var game = await _gamesRepository.Get(id);
        return Ok(game);
    }
}