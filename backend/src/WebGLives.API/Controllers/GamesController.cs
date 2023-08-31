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
    public async Task<IActionResult> All() =>
        Ok(_gamesRepository.All());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id) => 
        Ok(_gamesRepository.GetById(id));
}