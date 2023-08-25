using Microsoft.AspNetCore.Mvc;
using WebGLives.API.Services;
using WebGLives.DataAccess.Repositories;

namespace WebGLives.API.Controllers;

[ApiController]
[Route("[controller]")]
public class GamePagesController : ControllerBase
{
    private readonly IGamePageRepository _gamePagesRepository;

    public GamePagesController(IGamePageRepository gamePagesRepository) =>
        _gamePagesRepository = gamePagesRepository;

    [HttpGet]
    public async Task<IActionResult> All() =>
        Ok(_gamePagesRepository.All());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id) => 
        Ok(_gamePagesRepository.GetById(id));
}