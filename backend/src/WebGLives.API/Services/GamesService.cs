using WebGLives.API.Requests;
using WebGLives.Core;
using WebGLives.DataAccess.Repositories;

namespace WebGLives.API.Services;

public class GamesService : IGamesService
{
    private readonly IGamesRepository _repository;

    public async Task<IEnumerable<Game>> All() =>
        await _repository.All();

    public async Task Create(Game newGame) =>
        await _repository.Create(newGame);

    public Task<Game> Get(int id) =>
        _repository.GetOrDefault(id);

    public Task<bool> Delete(int id) =>
        _repository.Delete(id);
}