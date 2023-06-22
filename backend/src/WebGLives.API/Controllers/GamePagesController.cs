using Microsoft.AspNetCore.Mvc;
using WebGLives.API.Services;

namespace WebGLives.API.Controllers;

[ApiController]
[Route("[controller]")]
public class GamePagesController : ControllerBase
{
    private readonly IGamePagesRepository _gamePagesRepository;

    public GamePagesController(IGamePagesRepository gamePagesRepository) =>
        _gamePagesRepository = gamePagesRepository;

    [HttpGet]
    public async Task<IActionResult> All() =>
        Ok(_gamePagesRepository.All());
}